using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class RegRequestRepository(ApplicationDbContext context) : IRegRequestRepository
{
    public async Task<RegistrationRequest> AddAsync(RegistrationRequest registrationRequest,
        CancellationToken cancellationToken)
    {
        var entry = await context.RegistrationRequests
            .AddAsync(registrationRequest, cancellationToken);

        return entry.Entity;
    }

    public async Task<RegistrationRequest?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.RegistrationRequests
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<RegistrationRequest>, PaginationMetadata)> GetAllAsync(int eventId,
        GetAllRegRequestQueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        var query = context
            .RegistrationRequests
            .Include(r => r.Attendee)
            .Where(rr => rr.EventId == eventId)
            .AsQueryable();

        query = ApplyFilters(queryParameters, query);

        query = SortingHelper.ApplySorting(query, queryParameters.SortOrder,
            SortingHelper.RegRequestsSortingKeySelector(queryParameters.SortColumn));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
            queryParameters.PageIndex, queryParameters.PageSize, cancellationToken);

        query = PaginationHelper.ApplyPagination(query, queryParameters.PageIndex, queryParameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    private static IQueryable<RegistrationRequest> ApplyFilters(
        GetAllRegRequestQueryParameters queryParameters, IQueryable<RegistrationRequest> query)
    {
        if (queryParameters.AttendeeId.HasValue)
        {
            query = query.Where(r => r.AttendeeId == queryParameters.AttendeeId);
        }

        if (queryParameters.Status.HasValue)
        {
            query = query.Where(r => r.Status == queryParameters.Status);
        }

        return query;
    }
}
