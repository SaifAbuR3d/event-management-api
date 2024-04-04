using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetTicketsAsync(int eventId, CancellationToken cancellationToken);
    Task<Ticket?> GetTicketAsync(int ticketId, CancellationToken cancellationToken);

    /// <summary>
    /// Get available tickets count
    /// </summary>
    /// <param name="ticketId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetAvailableTicketsCountAsync(int ticketId, CancellationToken cancellationToken);
}
