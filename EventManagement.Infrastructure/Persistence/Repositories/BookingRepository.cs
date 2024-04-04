using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Domain.Entities;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class BookingRepository(ApplicationDbContext context) : IBookingRepository
{
    public async Task<Booking> AddBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        var entry = await context.Bookings.AddAsync(booking, cancellationToken);
        return entry.Entity;
    }

    public Task<Booking?> GetBookingByIdAsync(int bookingId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Booking>> GetBookingsByAttendeeIdAsync(int attendeeId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(int eventId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Booking>> GetBookingsByOrganizerIdAsync(int organizerId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
