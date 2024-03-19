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
            "id" => e => e.Id,
            _ => e => e.Id
        }; 
    }
    
}
