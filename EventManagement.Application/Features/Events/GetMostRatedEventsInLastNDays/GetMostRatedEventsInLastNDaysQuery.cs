using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Events.GetMostRatedEventsInTheLastNDays;

public record GetMostRatedEventsInLastNDaysQuery(int Days, int NumberOfEvents)
    : IRequest<IEnumerable<EventDto>>;

public class GetMostRatedEventsInLastNDaysQueryHandler(IEventRepository eventRepository,
    IUserRepository userRepository, ICurrentUser currentUser, IReviewRepository reviewRepository,
    IAttendeeRepository attendeeRepository, IMapper mapper)
    : IRequestHandler<GetMostRatedEventsInLastNDaysQuery, IEnumerable<EventDto>>
{
    public async Task<IEnumerable<EventDto>> Handle(GetMostRatedEventsInLastNDaysQuery request, CancellationToken cancellationToken)
    {
        var events = await eventRepository.GetMostRatedEventsInLastNDays(request.Days, request.NumberOfEvents,
            cancellationToken); 

        var eventsDto = mapper.Map<IEnumerable<EventDto>>(events);

        if (currentUser.IsAuthenticated && currentUser.IsAttendee)
        {
            var attendeeId = await userRepository.GetIdByUserId(currentUser.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), currentUser.UserId);

            foreach (var eventDto in eventsDto)
            {
                eventDto.IsLikedByCurrentUser = await attendeeRepository.DoesLikeEvent(int.Parse(attendeeId),
                                       eventDto.Id, cancellationToken);
            }
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
