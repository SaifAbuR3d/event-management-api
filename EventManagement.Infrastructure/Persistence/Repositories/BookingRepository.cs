using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class BookingRepository(ApplicationDbContext context) : IBookingRepository
{
    public async Task<Booking> AddBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        var entry = await context.Bookings.AddAsync(booking, cancellationToken);
        return entry.Entity;
    }

    public async Task<(IEnumerable<Booking>, PaginationMetadata)> GetBookingsAsync(
        int eventId, GetAllBookingsQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        var query = context.Bookings
            .Include(b => b.BookingTickets)
                 .ThenInclude(bt => bt.Ticket)
            .Where(b => b.EventId == eventId)
            .AsQueryable();

        query = ApplyFilters(query, queryParameters);

        query = SortingHelper.ApplySorting(query, queryParameters.SortOrder,
                       SortingHelper.BookingsSortingKeySelector(queryParameters.SortColumn));

        query = PaginationHelper.ApplyPagination(query, queryParameters.PageIndex, queryParameters.PageSize);

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
                       queryParameters.PageIndex, queryParameters.PageSize, cancellationToken);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    private static IQueryable<Booking> ApplyFilters(IQueryable<Booking> query,
               GetAllBookingsQueryParameters queryParameters)
    {
        if (queryParameters.AttendeeId.HasValue)
        {
            query = query.Where(b => b.AttendeeId == queryParameters.AttendeeId);
        }

        return query;
    }

    public async Task<IEnumerable<Booking>> GetLastBookings(int eventId,
        int count, CancellationToken cancellationToken)
    {
        var bookings = await context.Bookings
           .Include(b => b.Attendee)
           .Include(b => b.BookingTickets)
           .Where(b => b.EventId == eventId)
           .OrderByDescending(b => b.CreationDate)
           .ToListAsync(cancellationToken);

        var lastDistinctBookings = bookings
           .DistinctBy(b => b.Attendee)
           .Take(count);

        return lastDistinctBookings;
    }



}
