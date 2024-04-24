using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Like.LikeAnEvent;

public record LikeAnEventCommand(int EventId) : IRequest<Unit>;

public class LikeAnEventCommandHandler(ICurrentUser currentUser, IEventRepository eventRepository,
    IAttendeeRepository attendeeRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<LikeAnEventCommand, Unit>
{
    public async Task<Unit> Handle(LikeAnEventCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAttendee)
        {
            throw new UnauthorizedException("Only attendees can like events");
        }

        var attendee = await attendeeRepository.GetAttendeeByUserIdAsync(currentUser.UserId,
            cancellationToken) ?? throw new NotFoundException(nameof(Attendee), "UserId", currentUser.UserId);

        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        var alreadyLiked = await attendeeRepository.DoesLikeEvent(attendee.Id, @event.Id, cancellationToken);

        if (alreadyLiked)
        {
            throw new BadRequestException("Attendee has already liked this event");
        }

        attendee.Like(@event);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

