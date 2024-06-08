using EventManagement.Domain.Entities;
using System.Linq.Expressions;

namespace EventManagement.Infrastructure.Persistence.Helpers;

public class SortingHelper
{
    // Default behavior is to sort ascending
    public static IQueryable<T> ApplySorting<T>(IQueryable<T> query,
        string? sortOrder, Expression<Func<T, object>> keySelector)
    {
        return (sortOrder ?? "") == "desc" ?
            query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
    }
    
    public static Expression<Func<Event, object>> EventsSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => e => e.Name,
            "startdate" => e => e.StartDate,
            "enddate" => e => e.EndDate,
            "ticketscount" => e => e.Tickets.Count,
            _ => e => e.Id
        }; 
    }

    public static Expression<Func<Attendee, object>> AttendeesSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "userid" => a => a.UserId,
            "creationdate" => a => a.CreationDate,
            "lastmodified" => a => a.LastModified,
            "dateofbirth" => a => a.DateOfBirth,
            _ => a => a.Id
        };
    }

    public static Expression<Func<Organizer, object>> OrganizersSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "displayname" => o => o.DisplayName,
            "userid" => o => o.UserId,
            "creationdate" => o => o.CreationDate,
            "lastmodified" => o => o.LastModified,
            "isverified" => o => o.IsVerified,
            _ => o => o.Id
        };
    }

    public static Expression<Func<Review, object>> ReviewsSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "rating" => r => r.Rating,
            "creationdate" => r => r.CreationDate,
            "lastmodified" => r => r.LastModified,
            "comment" => r => r.Comment,
            _ => r => r.Id
        };
    }

    public static Expression<Func<Report, object>> ReportsSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "status" => r => r.Status,
            "creationdate" => r => r.CreationDate,
            "lastmodified" => r => r.LastModified,
            "content" => r => r.Content,
            _ => r => r.Id
        };
    }

    public static Expression<Func<IdentityVerificationRequest, object>> IvrSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "status" => ivr => ivr.Status,
            "creationdate" => ivr => ivr.CreationDate,
            "lastmodified" => ivr => ivr.LastModified,
            "userid" => ivr => ivr.UserId,
            _ => ivr => ivr.Id
        };
    }

    public static Expression<Func<RegistrationRequest, object>> RegRequestsSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "status" => rr => rr.Status,
            "creationdate" => rr => rr.CreationDate,
            "lastmodified" => rr => rr.LastModified,
            "attendeeid" => rr => rr.AttendeeId,
            "eventid" => rr => rr.EventId,
            _ => rr => rr.Id
        };
    }

    public static Expression<Func<Booking, object>> BookingsSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "creationdate" => b => b.CreationDate,
            "lastmodified" => b => b.LastModified,
            "attendeeid" => b => b.AttendeeId,
            "eventid" => b => b.EventId,
            _ => b => b.Id
        };
    }
    
}