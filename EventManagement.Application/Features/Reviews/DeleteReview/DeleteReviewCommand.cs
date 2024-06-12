using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Reviews.DeleteReview;
public record DeleteReviewCommand(int EventId, int ReviewId) : IRequest<Unit>;

public class DeleteReviewCommandHandler(ICurrentUser currentUser, 
    IReviewRepository reviewRepository, IEventRepository eventRepository, 
    IUnitOfWork unitOfWork, IUserRepository userRepository) : IRequestHandler<DeleteReviewCommand, Unit>
{
    public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        if(currentUser.IsOrganizer)
        {
            throw new UnauthorizedException("Organizers cannot delete reviews");
        }

        var review = await reviewRepository.GetReviewByIdAsync(request.ReviewId, cancellationToken)
            ?? throw new NotFoundException(nameof(Review), request.ReviewId);

        if(currentUser.IsAttendee)
        {
            await AuthorizeAttendee(review, cancellationToken);
        }

        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        if (review.EventId != @event.Id)
        {
            throw new BadRequestException("Review does not belong to the event");
        }

        reviewRepository.DeleteReview(review);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task AuthorizeAttendee(Review review, CancellationToken cancellationToken)
    {
        var currentUserId = currentUser.UserId;
        var currentUserAttendeeId = int.Parse(await userRepository.GetIdByUserId(currentUserId,
            cancellationToken) ?? throw new CustomException("Invalid State: No AttendeeId for the current user"));

        if (currentUserAttendeeId != review.AttendeeId)
        {
            throw new UnauthorizedException("Attendee can only delete their own reviews");
        }
    }
}

