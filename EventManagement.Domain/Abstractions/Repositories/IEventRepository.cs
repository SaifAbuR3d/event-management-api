using EventManagement.Domain.Models;

namespace EventManagement.Domain.Abstractions.Repositories;

public interface IEventRepository
{
    Task<Event> AddEventAsync(Event @event, CancellationToken cancellationToken); 
    Task<Event?> GetEventByIdAsync(int eventId, CancellationToken cancellationToken);

}
