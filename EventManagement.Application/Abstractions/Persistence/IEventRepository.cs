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

    Task<IEnumerable<Event>> GetEventsMayLikeByEventAsync(int eventId, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetEventsMayLikeByAttendee(int attendeeId, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetEventsMayLikeByAttendeeAndEventAsync(int eventId, int attendeeId,
        CancellationToken cancellationToken);

    Task<IEnumerable<Event>> GetNearEventsAsync(double latitude, double longitude, 
        int MaximumDistanceInKM, int NumberOfEvents);
    Task<IEnumerable<Event>> GetMostRatedEventsInLastNDays(int days, int numberOfEvents, CancellationToken cancellationToken);
}
