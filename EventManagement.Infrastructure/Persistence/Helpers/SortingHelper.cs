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
            _ => a => a.Id
        };
    }

    public static Expression<Func<Organizer, object>> OrganizersSortingKeySelector(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "displayName" => o => o.DisplayName,
            _ => o => o.Id
        };
    }
    
}
