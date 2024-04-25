using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Reviews.AddReview;

// Title is ignored for now
public record AddReviewCommand(int EventId, int Rating, string? Title, string Comment)
    : IRequest<int>;

public class AddReviewCommandHandler(ICurrentUser currentUser,
    IUnitOfWork unitOfWork, IAttendeeRepository attendeeRepository,
    IEventRepository eventRepository,
    IReviewRepository reviewRepository)
    : IRequestHandler<AddReviewCommand, int>
{
    public async Task<int> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAttendee)
        {
            throw new UnauthorizedException("Only attendees can add reviews");
        }

        var attendee = await attendeeRepository.GetAttendeeByUserIdAsync(currentUser.UserId,
            cancellationToken) ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId),
            currentUser.UserId);

        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken) ??
            throw new NotFoundException(nameof(Event), request.EventId);

        ValidateDateTime(@event);

        await ValidateAttendeeAuthority(attendee, @event, cancellationToken);

        var review = new Review(request.Rating, request.Comment, request.EventId, attendee.Id);

        await reviewRepository.AddReviewAsync(review, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return review.Id;
    }

    private async Task ValidateAttendeeAuthority(Attendee attendee, Event @event,
        CancellationToken cancellationToken)
    {
        var existingReview = await reviewRepository.GetReviewByAttendeeIdAndEventIdAsync(attendee.Id,
            @event.Id, cancellationToken);

        if (existingReview != null)
        {
            throw new BadRequestException("Attendee has already added a review for this event");
        }

        // commented out for testing purposes

        //var hasAttendedTheEvent = await attendeeRepository.HasAttendedEvent(attendee.Id, @event.Id,
        //    cancellationToken);

        //if (!hasAttendedTheEvent)
        //{
        //    throw new UnauthorizedException("Attendee has not attended the event");
        //}
    }

    private static void ValidateDateTime(Event @event)
    {
        var eventStartDateTime = new DateTime(@event.StartDate.Year, @event.StartDate.Month,
            @event.StartDate.Day, @event.StartTime.Hour, @event.StartTime.Minute, @event.StartTime.Second);

        if (eventStartDateTime > DateTime.UtcNow)
        {
            throw new BadRequestException("Event has not started yet");
        }
    }
}
