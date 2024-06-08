using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using MediatR;

namespace EventManagement.Application.Features.Events.GetOtherEventsMayLike;
public record GetOtherEventsMayLikeByAttendeeQuery() : IRequest<IEnumerable<EventDto>>;

public class GetOtherEventsMayLikeByAttendeeQueryHandler(ICurrentUser currentUser,
    IEventRepository eventRepository, IMapper mapper,
    IAttendeeRepository attendeeRepository,IUserRepository userRepository)
    : IRequestHandler<GetOtherEventsMayLikeByAttendeeQuery, IEnumerable<EventDto>>
{
    public async Task<IEnumerable<EventDto>> Handle(GetOtherEventsMayLikeByAttendeeQuery request, CancellationToken cancellationToken)
    {
        if(!currentUser.IsAuthenticated || !currentUser.IsAttendee)
        {
            throw new UnauthorizedException("User is not an attendee");
        }

        var attendeeId = await userRepository.GetIdByUserId(currentUser.UserId, cancellationToken)
            ?? throw new CustomException("Invalid State: this userId must be attached to an attendee");

        var events = await eventRepository.GetEventsMayLikeByAttendee(int.Parse(attendeeId), cancellationToken);

        var eventsDto = mapper.Map<IEnumerable<EventDto>>(events);

        foreach (var eventDto in eventsDto)
        {
            eventDto.IsLikedByCurrentUser = await attendeeRepository.DoesLikeEvent(int.Parse(attendeeId),
                eventDto.Id, cancellationToken);
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
