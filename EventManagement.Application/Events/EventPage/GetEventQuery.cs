using AutoMapper;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Models;
using MediatR;

namespace EventManagement.Application.Events.EventPage;

public record GetEventQuery(int EventId) : IRequest<EventDto>;

public class GetEventQueryHandler(IEventRepository eventRepository,
    IMapper mapper) : IRequestHandler<GetEventQuery, EventDto>
{

    public async Task<EventDto> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken) 
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        return mapper.Map<EventDto>(@event);
    }
}