using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Domain.Entities;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class TicketRepository(ApplicationDbContext context) : ITicketRepository
{
    public async Task<Ticket?> GetTicketAsync(int ticketId, CancellationToken cancellationToken)
    {
        return await context.Tickets.FindAsync(ticketId, cancellationToken);
    }

    public Task<int> GetAvailableTicketsCountAsync(int ticketId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Ticket>> GetTicketsAsync(int eventId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
