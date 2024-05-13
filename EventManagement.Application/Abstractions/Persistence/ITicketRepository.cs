using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

/// <summary>
/// repository for Ticket Entity (tickets types), and booking ticket (single ticket)
/// </summary>
public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetTicketsTypesAsync(int eventId, CancellationToken cancellationToken);
    Task<Ticket?> GetTicketTypeAsync(int ticketId, CancellationToken cancellationToken);
    Task<BookingTicket?> GetBookingTicketAsync(int eventId, Guid checkInCode,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get available tickets count
    /// </summary>
    /// <param name="ticketId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    //Task<int> GetAvailableTicketsCountAsync(int ticketId, CancellationToken cancellationToken);
}
