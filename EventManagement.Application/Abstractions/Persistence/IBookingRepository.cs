using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IBookingRepository
{
    Task<Booking> AddBookingAsync(Booking booking, CancellationToken cancellationToken);
    Task<Booking?> GetBookingByIdAsync(int bookingId, CancellationToken cancellationToken);
    Task<(IEnumerable<Booking>, PaginationMetadata)> GetBookingsAsync(int eventId,
               GetAllBookingsQueryParameters queryParameters, CancellationToken cancellationToken);

    Task<IEnumerable<Booking>> GetBookingsByAttendeeIdAsync(int attendeeId,
        CancellationToken cancellationToken);
    Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(int eventId,
        CancellationToken cancellationToken);
    Task<IEnumerable<Booking>> GetBookingsByOrganizerIdAsync(int organizerId,
        CancellationToken cancellationToken);
}
