﻿using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Events.GetEvent;

public record GetEventQuery(int EventId) : IRequest<EventDto>;

public class GetEventQueryHandler(IEventRepository eventRepository, IUserRepository userRepository,
    ICurrentUser currentUser,IAttendeeRepository attendeeRepository,
    IMapper mapper) : IRequestHandler<GetEventQuery, EventDto>
{

    public async Task<EventDto> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        var organizerImageUrl = await userRepository.GetProfilePictureByUserId(@event.Organizer.UserId,
            cancellationToken);

        var eventDto = mapper.Map<EventDto>(@event);

        eventDto.Organizer.ImageUrl = organizerImageUrl;
        eventDto.Organizer.UserName = await userRepository.GetUserNameByUserId(@event.Organizer.UserId,
            cancellationToken) ?? throw new CustomException("Invalid State: Organizer has no UserName");

        // If the organizer does not have a profile, return an empty profile
        if (eventDto.Organizer.Profile == null)
        {
            eventDto.Organizer.Profile = new ProfileDto();
        }

        if (currentUser.IsAuthenticated && currentUser.IsAttendee)
        {
            var attendeeId = await userRepository.GetIdByUserId(currentUser.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), currentUser.UserId);

            eventDto.IsLikedByCurrentUser = await attendeeRepository.DoesLikeEvent(int.Parse(attendeeId),
                @event.Id, cancellationToken);
        }

        return eventDto;
    }
}