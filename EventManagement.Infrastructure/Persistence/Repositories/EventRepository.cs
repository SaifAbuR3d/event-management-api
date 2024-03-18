using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class EventRepository(ApplicationDbContext context) : IEventRepository
{


    public async Task<Event> AddEventAsync(Event @event, CancellationToken cancellationToken)
    {
        var entry = await context.Events.AddAsync(@event, cancellationToken);
        return entry.Entity;
    }

    public async Task<Event?> GetEventByIdAsync(int eventId, CancellationToken cancellationToken)
    {
        return await context.Events
            .Include(e => e.Organizer)
            .Include(e => e.EventImages)
            .Include(e => e.Tickets)
            .Include(e => e.Categories)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
    }
}
