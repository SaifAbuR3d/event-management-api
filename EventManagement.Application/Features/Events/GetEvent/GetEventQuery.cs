using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Events.GetEvent;

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