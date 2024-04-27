using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

internal class IvrRepository(ApplicationDbContext context) : IIvrRepository
{
    public async Task<IdentityVerificationRequest> AddAsync(IdentityVerificationRequest ivr, CancellationToken cancellationToken)
    {
        var entry = await context.IdentityVerificationRequests.AddAsync(ivr, cancellationToken);
        return entry.Entity;
    }

    public async Task<bool> HasPendingRequests(int userId, CancellationToken cancellationToken)
    {
        return await context.IdentityVerificationRequests
                    .AnyAsync(ivr => ivr.UserId == userId
                            && ivr.Status == IdentityVerificationRequestStatus.Pending,
                            cancellationToken);

    }

    public async Task<(IEnumerable<IdentityVerificationRequest>, PaginationMetadata)> GetAllAsync
        (GetAllIvrsQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        var query = context
            .IdentityVerificationRequests
            .Include(ivr => ivr.Document)
            .AsQueryable();

        query = ApplyFilters(query, queryParameters);

        query = SortingHelper.ApplySorting(query, queryParameters.SortOrder,
            SortingHelper.IvrSortingKeySelector(queryParameters.SortColumn));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
            queryParameters.PageIndex, queryParameters.PageSize, cancellationToken);

        query = PaginationHelper.ApplyPagination(query, queryParameters.PageIndex, queryParameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    private IQueryable<IdentityVerificationRequest> ApplyFilters(
        IQueryable<IdentityVerificationRequest> query, GetAllIvrsQueryParameters queryParameters)
    {
        if (queryParameters.UserId.HasValue)
        {
            query = query.Where(ivr => ivr.UserId == queryParameters.UserId);
        }

        if (queryParameters.OnlyAttendees.HasValue)
        {
            query = query.Where(ivr => !ivr.IsForOrganizer);
        }

        if (queryParameters.OnlyOrganizers.HasValue)
        {
            query = query.Where(ivr => ivr.IsForOrganizer);
        }

        if (queryParameters.Status.HasValue)
        {
            query = query.Where(ivr => ivr.Status == queryParameters.Status);
        }

        return query;
    }

    public Task<IdentityVerificationRequest?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return context.IdentityVerificationRequests
            .Include(ivr => ivr.Document)
            .FirstOrDefaultAsync(ivr => ivr.Id == id, cancellationToken);
    }

    public Task<IdentityVerificationRequest?> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return context.IdentityVerificationRequests
            .Include(ivr => ivr.Document)
            .FirstOrDefaultAsync(ivr => ivr.UserId == userId, cancellationToken);
    }
}
