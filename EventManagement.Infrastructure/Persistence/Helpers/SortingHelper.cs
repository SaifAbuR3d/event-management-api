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
            "startDate" => e => e.StartDate,
            "endDate" => e => e.EndDate,
            "ticketsCount" => e => e.Tickets.Count,
            _ => e => e.Id
        }; 
    }

    public static Expression<Func<Attendee, object>> AttendeesSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "userId" => a => a.UserId,
            "creationDate" => a => a.CreationDate,
            "lastModified" => a => a.LastModified,
            "dateOfBirth" => a => a.DateOfBirth,
            _ => a => a.Id
        };
    }

    public static Expression<Func<Organizer, object>> OrganizersSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "displayName" => o => o.DisplayName,
            "userId" => o => o.UserId,
            "creationDate" => o => o.CreationDate,
            "lastModified" => o => o.LastModified,
            "isVerified" => o => o.IsVerified,
            _ => o => o.Id
        };
    }

    public static Expression<Func<Review, object>> ReviewsSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "rating" => r => r.Rating,
            "creationDate" => r => r.CreationDate,
            "lastModified" => r => r.LastModified,
            "comment" => r => r.Comment,
            _ => r => r.Id
        };
    }

    public static Expression<Func<Report, object>> ReportsSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "status" => r => r.Status,
            "creationDate" => r => r.CreationDate,
            "lastModified" => r => r.LastModified,
            "content" => r => r.Content,
            _ => r => r.Id
        };
    }

    public static Expression<Func<IdentityVerificationRequest, object>> IvrSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "status" => ivr => ivr.Status,
            "creationDate" => ivr => ivr.CreationDate,
            "lastModified" => ivr => ivr.LastModified,
            "userId" => ivr => ivr.UserId,
            _ => ivr => ivr.Id
        };
    }

    public static Expression<Func<RegistrationRequest, object>> RegRequestsSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "status" => rr => rr.Status,
            "creationDate" => rr => rr.CreationDate,
            "lastModified" => rr => rr.LastModified,
            "attendeeId" => rr => rr.AttendeeId,
            "eventId" => rr => rr.EventId,
            _ => rr => rr.Id
        };
    }

    public static Expression<Func<Booking, object>> BookingsSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "creationDate" => b => b.CreationDate,
            "lastModified" => b => b.LastModified,
            "attendeeId" => b => b.AttendeeId,
            "eventId" => b => b.EventId,
            _ => b => b.Id
        };
    }
    
}