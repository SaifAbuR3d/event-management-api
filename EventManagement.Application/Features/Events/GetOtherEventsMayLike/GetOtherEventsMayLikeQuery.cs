using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Events.GetOtherEventsMayLike;

public record GetOtherEventsMayLikeQuery(int EventId) : IRequest<IEnumerable<EventDto>>;

public class GetOtherEventsMayLikeQueryHandler(ICurrentUser currentUser,
    IEventRepository eventRepository, IMapper mapper,
    IUserRepository userRepository)
    : IRequestHandler<GetOtherEventsMayLikeQuery, IEnumerable<EventDto>>
{
    public async Task<IEnumerable<EventDto>> Handle(GetOtherEventsMayLikeQuery request, CancellationToken cancellationToken)
    {
        // TODO: Get the current user's ID if authenticated,
        // we can use this to get events that the user may like (based on their interests, location, ...)
        // especially if the user is Attendee

        IEnumerable<Event> events = [];

        if (currentUser.IsAuthenticated && currentUser.IsAttendee)
        // used additional check for isAuthenticated to avoid throwing exception when the user is not authenticated
        {
            var userId = currentUser.UserId;
            var attendeeId = await userRepository.GetIdByUserId(userId, cancellationToken)
                ?? throw new CustomException("Invalid State: this userId must be attached to an attendee");
            var attendeeIdInt = int.Parse(attendeeId);
            events = await eventRepository.GetEventsMayLikeForAttendeeAsync(request.EventId, attendeeIdInt, cancellationToken);
        }
        else
        {
            events = await eventRepository.GetEventsMayLikeAsync(request.EventId, cancellationToken);
        }

        return mapper.Map<IEnumerable<EventDto>>(events);
    }
}
