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
    IEventRepository eventRepository, IMapper mapper, IAttendeeRepository attendeeRepository,
    IUserRepository userRepository)
    : IRequestHandler<GetOtherEventsMayLikeQuery, IEnumerable<EventDto>>
{
    public async Task<IEnumerable<EventDto>> Handle(GetOtherEventsMayLikeQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Event> events = [];
        IEnumerable<EventDto> eventsDto = [];

        if (currentUser.IsAuthenticated && currentUser.IsAttendee)
        // used additional check for isAuthenticated to avoid throwing exception when the user is not authenticated
        {
            var userId = currentUser.UserId;
            var attendeeId = await userRepository.GetIdByUserId(userId, cancellationToken)
                ?? throw new CustomException("Invalid State: this userId must be attached to an attendee");
            events = await eventRepository.GetEventsMayLikeByAttendeeAndEventAsync(request.EventId,
                int.Parse(attendeeId), cancellationToken);

            eventsDto = mapper.Map<IEnumerable<EventDto>>(events);

            foreach (var eventDto in eventsDto)
            {
                eventDto.IsLikedByCurrentUser = await attendeeRepository.DoesLikeEvent(int.Parse(attendeeId),
                    eventDto.Id, cancellationToken);
            }

        }
        else
        {
            events = await eventRepository.GetEventsMayLikeByEventAsync(request.EventId, cancellationToken);
            eventsDto = mapper.Map<IEnumerable<EventDto>>(events);
        }

        foreach (var eventDto in eventsDto)
        {
            eventDto.Organizer.ImageUrl = await userRepository.GetProfilePictureByUserId(
                eventDto.Organizer.UserId, cancellationToken);

            eventDto.Organizer.UserName = await userRepository.GetUserNameByUserId(
                               eventDto.Organizer.UserId, cancellationToken)
                ?? throw new CustomException("Invalid State: Organizer has no UserName");

            if (eventDto.Organizer.Profile == null)
            {
                eventDto.Organizer.Profile = new ProfileDto();
            }
        }

        return eventsDto;
    }
}
