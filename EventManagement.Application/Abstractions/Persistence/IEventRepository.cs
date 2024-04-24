using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IEventRepository
{
    Task<Event> AddEventAsync(Event @event, CancellationToken cancellationToken);
    Task<Event?> GetEventByIdAsync(int eventId, CancellationToken cancellationToken);
    Task<(IEnumerable<Event>, PaginationMetadata)> GetEventsAsync(GetAllEventsQueryParameters queryParameters,
        CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetEventsMayLikeAsync(int eventId, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetEventsMayLikeForAttendeeAsync(int eventId, int attendeeId,
        CancellationToken cancellationToken);
}
