﻿using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using MediatR;

namespace EventManagement.Application.Events.GetAllEvents;

public record GetAllEventsQuery(GetAllEventsQueryParameters Parameters)
    : IRequest<(IEnumerable<EventDto>, PaginationMetadata)>;

public class GetAllEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
    : IRequestHandler<GetAllEventsQuery, (IEnumerable<EventDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<EventDto>, PaginationMetadata)> Handle(GetAllEventsQuery request,
        CancellationToken cancellationToken)
    {
        var (events, paginationMetadata) = await eventRepository.GetEventsAsync(request.Parameters, cancellationToken);
        var eventsDto = mapper.Map<IEnumerable<EventDto>>(events);
        return (eventsDto, paginationMetadata);
    }
}