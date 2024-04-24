﻿using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using MediatR;

namespace EventManagement.Application.Features.Events.GetAllEvents;

public record GetAllEventsQuery(GetAllEventsQueryParameters Parameters)
    : IRequest<(IEnumerable<EventDto>, PaginationMetadata)>;

public class GetAllEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
    : IRequestHandler<GetAllEventsQuery, (IEnumerable<EventDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<EventDto>, PaginationMetadata)> Handle(GetAllEventsQuery request,
        CancellationToken cancellationToken)
    {
        ValidateParameters(request);

        var (events, paginationMetadata) = await eventRepository.GetEventsAsync(request.Parameters, cancellationToken);
        var eventsDto = mapper.Map<IEnumerable<EventDto>>(events);
        return (eventsDto, paginationMetadata);
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