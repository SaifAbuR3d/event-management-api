using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Events.GetEventsFromFollowingOrganizers;
public record GetEventsFromFollowingOrganizersQuery(
    GetAllEventsFromFollowingOrganizersQueryParameters Parameters)
    : IRequest<(IEnumerable<EventDto>, PaginationMetadata)>;

public class GetEventsFromFollowingOrganizersQueryHandler(IEventRepository eventRepository,
    IMapper mapper, ICurrentUser currentUser,
    IAttendeeRepository attendeeRepository, IUserRepository userRepository)
    : IRequestHandler<GetEventsFromFollowingOrganizersQuery, (IEnumerable<EventDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<EventDto>, PaginationMetadata)> Handle(
        GetEventsFromFollowingOrganizersQuery request, CancellationToken cancellationToken)
    {
        if(!currentUser.IsAttendee)
        {
            throw new UnauthorizedException("Only attendees can get events from following organizers");
        }

        var attendeeId = int.Parse(await userRepository.GetIdByUserId(currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), currentUser.UserId));

        var (events, paginationMetadata) = await eventRepository.GetEventsFromFollowingOrganizersAsync(
                       request.Parameters, attendeeId, cancellationToken);

        var eventsDto = mapper.Map<IEnumerable<EventDto>>(events);

        foreach (var eventDto in eventsDto)
        {
            eventDto.IsLikedByCurrentUser = await attendeeRepository.DoesLikeEvent(attendeeId,
                                   eventDto.Id, cancellationToken);
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

        return (eventsDto, paginationMetadata);
    }
}

