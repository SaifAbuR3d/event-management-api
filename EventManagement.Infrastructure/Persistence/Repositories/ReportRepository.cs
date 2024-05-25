using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class ReportRepository(ApplicationDbContext context) : IReportRepository
{
    public async Task<int> AddReportAsync(Report report, CancellationToken cancellationToken)
    {
        var entry = await context.Reports.AddAsync(report, cancellationToken);
        return entry.Entity.Id;
    }

    public async Task<(IEnumerable<Report>, PaginationMetadata)> GetAllReportsAsync(
        GetAllReportsQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = context.Reports
            .Include(r => r.Attendee)
            .Include(r => r.Event)
                .ThenInclude(e => e.Organizer)
            .AsQueryable();

        query = ApplyFilters(parameters, query);

        query = SortingHelper.ApplySorting(query, parameters.SortOrder,
            SortingHelper.ReportsSortingKeySelector(parameters.SortColumn));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
            parameters.PageIndex, parameters.PageSize, cancellationToken);

        query = PaginationHelper.ApplyPagination(query, parameters.PageIndex, parameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    private static IQueryable<Report> ApplyFilters(GetAllReportsQueryParameters parameters, IQueryable<Report> query)
    {
        if (parameters.EventId.HasValue)
        {
            query = query.Where(r => r.EventId == parameters.EventId);
        }
        if (parameters.AttendeeId.HasValue)
        {
            query = query.Where(r => r.AttendeeId == parameters.AttendeeId);
        }
        if (parameters.Status.HasValue)
        {
            query = query.Where(r => r.Status == parameters.Status);
        }
        if (parameters.OrganizerId.HasValue)
        {
            query = query.Where(r => r.Event.OrganizerId == parameters.OrganizerId);
        }

        return query;
    }

    public async Task<Report?> GetReportByIdAsync(int reportId, CancellationToken cancellationToken)
    {
        return await context.Reports
            .Include(r => r.Attendee)
            .Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == reportId, cancellationToken);
    }

    public Task<(IEnumerable<Report>, PaginationMetadata)> GetReportsByEventIdAsync(int eventId, GetAllQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<Report>, PaginationMetadata)> GetReportsByAttendeeIdAsync(int attendeeId, GetAllQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
