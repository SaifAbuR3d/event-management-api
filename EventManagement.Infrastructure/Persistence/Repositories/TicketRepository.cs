using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class TicketRepository(ApplicationDbContext context) : ITicketRepository
{
    public async Task<Ticket?> GetTicketTypeAsync(int ticketId, CancellationToken cancellationToken)
    {
        return await context.Tickets.FindAsync(ticketId, cancellationToken);
    }

    public async Task<BookingTicket?> GetBookingTicketAsync(int eventId, Guid checkInCode, CancellationToken cancellationToken)
    {
        var ticket = await context.BookingTickets
            .Where(bt => bt.Ticket.EventId == eventId && bt.CheckInCode == checkInCode)
            .SingleOrDefaultAsync(cancellationToken);

        return ticket;
    }

    public async Task<IEnumerable<Ticket>> GetEventTickets(int eventId,
        CancellationToken cancellationToken)
    {
        return await context.Tickets
            .Where(t => t.EventId == eventId)
            .ToListAsync(cancellationToken);
    }

    // get the selling track of an event, (date, number of tickets sold) for each day since the event was created
    public async Task<IEnumerable<SellingRecord>> GetSellingTrack(int eventId, CancellationToken cancellationToken)
    {
        return await context.BookingTickets
            .Where(bt => bt.Booking.EventId == eventId)
            .GroupBy(bt => bt.CreationDate.Date)
            .Select(g => new SellingRecord(g.Key, g.Count()))
            .ToListAsync(cancellationToken);
    }
}
