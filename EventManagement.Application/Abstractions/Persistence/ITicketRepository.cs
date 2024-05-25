using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

/// <summary>
/// repository for Ticket Entity (tickets types), and booking ticket (single ticket)
/// </summary>
public interface ITicketRepository
{
    Task<Ticket?> GetTicketTypeAsync(int ticketId, CancellationToken cancellationToken);
    Task<BookingTicket?> GetBookingTicketAsync(int eventId, Guid checkInCode,
        CancellationToken cancellationToken);
    Task<IEnumerable<Ticket>> GetEventTickets(int eventId, CancellationToken cancellationToken);
    Task<IEnumerable<SellingRecord>> GetSellingTrack(int eventId, CancellationToken cancellationToken);
}
