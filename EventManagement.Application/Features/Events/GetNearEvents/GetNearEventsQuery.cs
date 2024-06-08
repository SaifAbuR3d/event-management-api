using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using static EventManagement.Domain.Constants.Location;
using MediatR;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Features.Events.GetNearEvents;

// TODO: Add IsTicketSalesRunning=true Query Parameter
public record GetNearEventsQuery(double Latitude, double Longitude,
    int MaximumDistanceInKM, int NumberOfEvents) : IRequest<IEnumerable<EventDto>>;

public class GetNearEventsQueryHandler(IEventRepository eventRepository, 
    IUserRepository userRepository, ICurrentUser currentUser,
    IAttendeeRepository attendeeRepository,IMapper mapper)
    : IRequestHandler<GetNearEventsQuery, IEnumerable<EventDto>>
{
    public async Task<IEnumerable<EventDto>> Handle(GetNearEventsQuery request, CancellationToken cancellationToken)
    {
        Validate(request);

        var events = await eventRepository.GetNearEventsAsync(request.Latitude, request.Longitude, request.MaximumDistanceInKM, request.NumberOfEvents);

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

    private static void Validate(GetNearEventsQuery request)
    {
        if (request.Latitude < MinLatitude || request.Latitude > MaxLatitude
            || request.Longitude < MinLongitude || request.Longitude > MaxLongitude)
        {
            throw new BadRequestException("Invalid latitude or longitude");
        }

        if (request.MaximumDistanceInKM <= 0)
        {
            throw new BadRequestException("Maximum distance should be greater than 0");
        }

        if (request.MaximumDistanceInKM > 10000)
        {
            throw new BadRequestException("Maximum distance should be less than 1000 km");
        }

        if (request.NumberOfEvents <= 0)
        {
            throw new BadRequestException("Number of events should be greater than 0");
        }

        if (request.NumberOfEvents > 100)
        {
            throw new BadRequestException("Number of events should be less than 100");
        }
    }
}
