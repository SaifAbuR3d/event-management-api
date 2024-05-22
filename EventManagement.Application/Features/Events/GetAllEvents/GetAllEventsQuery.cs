using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Events.GetAllEvents;

public record GetAllEventsQuery(GetAllEventsQueryParameters Parameters)
    : IRequest<(IEnumerable<EventDto>, PaginationMetadata)>;

public class GetAllEventsQueryHandler(IEventRepository eventRepository, IMapper mapper,
    ICurrentUser currentUser, IAttendeeRepository attendeeRepository, 
    IUserRepository userRepository) :
    IRequestHandler<GetAllEventsQuery, (IEnumerable<EventDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<EventDto>, PaginationMetadata)> Handle(GetAllEventsQuery request,
        CancellationToken cancellationToken)
    {
        ValidateParameters(request);
        await Authenticate(request, cancellationToken);

        var (events, paginationMetadata) = await eventRepository.GetEventsAsync(request.Parameters, cancellationToken);
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

        return (eventsDto, paginationMetadata);
    }

    private async Task Authenticate(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        if (request.Parameters.AttendeeId.HasValue)
        {
            if (!currentUser.IsAuthenticated || !currentUser.IsAttendee)
            {
                throw new UnauthorizedException("Only authenticated attendees can access their booked events.");
            }

            int currentAttendeeId = int.Parse(await userRepository.GetIdByUserId(currentUser.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), currentUser.UserId));

            if(currentAttendeeId != request.Parameters.AttendeeId)
            {
                throw new UnauthorizedException("Authenticated attendee can only access their own booked events.");
            }

        }
    }

    private static void ValidateParameters(GetAllEventsQuery request)
    {
        var count = 0;
        if (request.Parameters.PreviousEvents) count++;
        if (request.Parameters.UpcomingEvents) count++;
        if (request.Parameters.RunningEvents) count++;

        if (count > 1)
        {
            throw new BadRequestException("Only one of the PreviousEvents, UpcomingEvents," +
                " and RunningEvents parameters can be true.");
        }
    }
}