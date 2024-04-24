using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Like.RemoveLike;

public record RemoveLikeFromEventCommand(int EventId) : IRequest<Unit>;

public class RemoveLikeFromEventCommandHandler(ICurrentUser currentUser,
    IEventRepository eventRepository, IAttendeeRepository attendeeRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveLikeFromEventCommand, Unit>
{
    public async Task<Unit> Handle(RemoveLikeFromEventCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAttendee)
        {
            throw new UnauthorizedException("Only attendees can remove likes from events");
        }

        var attendee = await attendeeRepository.GetAttendeeByUserIdAsync(currentUser.UserId, cancellationToken) 
            ?? throw new NotFoundException(nameof(Attendee), "UserId", currentUser.UserId);

        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        var doesLike = await attendeeRepository.DoesLikeEvent(attendee.Id, @event.Id, cancellationToken);

        if (!doesLike)
        {
            throw new BadRequestException("Attendee has not liked this event");
        }

        await attendeeRepository.RemoveLikeFromEvent(attendee.Id, @event.Id, cancellationToken); 

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
