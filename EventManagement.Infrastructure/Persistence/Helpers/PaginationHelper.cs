using EventManagement.Application.Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Helpers;

public class PaginationHelper
{
    public static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int pageIndex, int pageSize)
    {
        return query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
    }

    public static async Task<PaginationMetadata> GetPaginationMetadataAsync<T>(IQueryable<T> query,
        int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var count = await query.CountAsync(cancellationToken);
        return new PaginationMetadata(pageIndex, pageSize, count);
    }
}
