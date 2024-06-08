using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.SqlClient;
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
                .ThenInclude(o => o.Profile)
            .Include(e => e.EventImages)
            .Include(e => e.Tickets)
            .Include(e => e.Categories)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
    }

    public async Task<(IEnumerable<Event>, PaginationMetadata)> GetEventsAsync(GetAllEventsQueryParameters queryParameters,
               CancellationToken cancellationToken)
    {
        var query = context.Events
            .Include(e => e.Organizer)
            .Include(e => e.EventImages)
            .Include(e => e.Tickets)
            .Include(e => e.Categories)
            .AsQueryable();

        query = await ApplyFilters(query, queryParameters);

        query = SortingHelper.ApplySorting(query, queryParameters.SortOrder,
            SortingHelper.EventsSortingKeySelector(queryParameters.SortColumn));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
            queryParameters.PageIndex, queryParameters.PageSize, cancellationToken);

        query = PaginationHelper.ApplyPagination(query, queryParameters.PageIndex, queryParameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    private async Task<IQueryable<Event>> ApplyFilters(IQueryable<Event> query,
        GetAllEventsQueryParameters queryParameters)
    {
        if (queryParameters.EventId.HasValue)
        {
            query = query.Where(e => e.Id == queryParameters.EventId);
        }

        if (queryParameters.CategoryId.HasValue)
        {
            query = query.Where(e => e.Categories.Any(c => c.Id == queryParameters.CategoryId));
        }

        if (queryParameters.OrganizerId.HasValue)
        {
            query = query.Where(e => e.Organizer.Id == queryParameters.OrganizerId);
        }

        // TODO: for more precise result, you can compare 'current datetime'
        // with 'new DateTime(event.startDate, event.startTime)'  ....


        if (queryParameters.PreviousEvents)
        {
            query = query.Where(e => e.EndDate < DateOnly.FromDateTime(DateTime.UtcNow));
        }

        if (queryParameters.UpcomingEvents)
        {
            query = query.Where(e => e.StartDate > DateOnly.FromDateTime(DateTime.UtcNow));
        }

        if (queryParameters.RunningEvents)
        {
            query = query.Where(e => e.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow)
                                  && e.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow));
        }

        if(queryParameters.LikedBy.HasValue)
        {
            var eventLikedByAttendee = await context.Likes
                .Where(l => l.AttendeeId == queryParameters.LikedBy)
                .Select(l => l.EventId)
                .ToListAsync();

            query = query.Where(e => eventLikedByAttendee.Contains(e.Id));
        }

        if(queryParameters.AttendeeId.HasValue)
        {
            var eventBookedByAttendee = await context.Bookings
                .Where(b => b.AttendeeId == queryParameters.AttendeeId)
                .Select(b => b.EventId)
                .ToListAsync();

            query = query.Where(e => eventBookedByAttendee.Contains(e.Id));
        }

        return query;
    }

    public async Task<IEnumerable<Event>> GetEventsMayLikeAsync(int eventId,
        CancellationToken cancellationToken)
    {
        var eventCategories = await context.Events
            .Where(e => e.Id == eventId)
            .SelectMany(e => e.Categories.Select(c => c.Id))
            .ToListAsync(cancellationToken);

        var events = await context.Events
            .Include(e => e.Categories)
            .Include(e => e.Organizer)
            .Include(e => e.EventImages)
            .Include(e => e.Tickets)
            .Where(e => e.Categories.Any(c => eventCategories.Contains(c.Id)))
            .Where(e => e.Id != eventId)
            .OrderBy(e => Guid.NewGuid())
            .Take(10)
            .ToListAsync(cancellationToken);

        return events;
    }



    // TODO: Implement this method, (combine attendee interests and location with event categories and location)
    public async Task<IEnumerable<Event>> GetEventsMayLikeForAttendeeAndEventAsync(int eventId,
    int attendeeId, CancellationToken cancellationToken)
    {
        return await GetEventsMayLikeAsync(eventId, cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetNearEventsAsync(double latitude,
        double longitude, int maximumDistanceInKM, int numberOfEvents)
    {

        //The distance between two points is calculated using the Haversine formula,
        //which determines the great-circle distance between two points on the Earth's surface.

        // the formula is: 
        // d = R * acos(sin(lat1) * sin(lat2) + cos(lat1) * cos(lat2) * cos(lon2 - lon1))
        // where:
        // d is the distance between the two points
        // R is the radius of the Earth (mean radius = 6,371 km)
        // lat1, lon1 are the coordinates of the first point
        // lat2, lon2 are the coordinates of the second point

        int radiusOfEarthInKM = 6371;

        var query = $@"
        SELECT TOP {numberOfEvents} Id,
               {radiusOfEarthInKM} *
               ACOS(COS(RADIANS({latitude})) * COS(RADIANS(Latitude)) * COS(RADIANS(Longitude) - RADIANS({longitude})) +
                    SIN(RADIANS({latitude})) * SIN(RADIANS(Latitude))
               ) AS Distance
        FROM Events
        WHERE {radiusOfEarthInKM} *
              ACOS(COS(RADIANS({latitude})) * COS(RADIANS(Latitude)) * COS(RADIANS(Longitude) - RADIANS({longitude})) +
                   SIN(RADIANS({latitude})) * SIN(RADIANS(Latitude))
              ) < {maximumDistanceInKM}
        ORDER BY Distance";

        // get the event IDs
        var eventIds = await context.Events
            .FromSqlRaw(query)
            .Select(e => e.Id)
            .ToListAsync();

        // Fetch the full event details
        return await context.Events
            .Include(e => e.Organizer)
            .Include(e => e.EventImages)
            .Include(e => e.Tickets)
            .Include(e => e.Categories)
            .Where(e => eventIds.Contains(e.Id))
            .ToListAsync();
    }



}
