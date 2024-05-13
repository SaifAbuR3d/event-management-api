using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class TicketRepository(ApplicationDbContext context) : ITicketRepository
{
    public async Task<Ticket?> GetTicketTypeAsync(int ticketId, CancellationToken cancellationToken)
    {
        return await context.Tickets.FindAsync(ticketId, cancellationToken);
    }

    public Task<int> GetAvailableTicketsCountAsync(int ticketId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Ticket>> GetTicketsTypesAsync(int eventId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<BookingTicket?> GetBookingTicketAsync(int eventId, Guid checkInCode, CancellationToken cancellationToken)
    {
        var ticket = await context.BookingTickets
            .Where(bt => bt.Ticket.EventId == eventId && bt.CheckInCode == checkInCode)
            .SingleOrDefaultAsync(cancellationToken);

        return ticket;
    }
}
