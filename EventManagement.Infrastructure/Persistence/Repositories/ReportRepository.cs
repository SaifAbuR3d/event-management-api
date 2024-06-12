using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class ReportRepository(ApplicationDbContext context) : IReportRepository
{
    public async Task<int> AddEventReportAsync(EventReport report, CancellationToken cancellationToken)
    {
        var entry = await context.EventReports.AddAsync(report, cancellationToken);
        return entry.Entity.Id;
    }
    public async Task<int> AddReviewReportAsync(ReviewReport report,
        CancellationToken cancellationToken)
    {
        var entry = await context.ReviewReports.AddAsync(report, cancellationToken);
        return entry.Entity.Id;
    }


    public async Task<(IEnumerable<EventReport>, PaginationMetadata)> GetAllEventReportsAsync(
        GetAllEventReportsQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = context.EventReports
            .Include(r => r.Attendee)
            .Include(r => r.Event)
                .ThenInclude(e => e.Organizer)
            .AsQueryable();

        query = ApplyFilters(parameters, query);

        query = SortingHelper.ApplySorting(query, parameters.SortOrder,
            SortingHelper.EventReportsSortingKeySelector(parameters.SortColumn));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
            parameters.PageIndex, parameters.PageSize, cancellationToken);

        query = PaginationHelper.ApplyPagination(query, parameters.PageIndex, parameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    private static IQueryable<EventReport> ApplyFilters(GetAllEventReportsQueryParameters parameters,
        IQueryable<EventReport> query)
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

    public async Task<(IEnumerable<ReviewReport>, PaginationMetadata)> GetAllReviewReportsAsync(
        GetAllReviewReportsQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = context.ReviewReports
            .Include(r => r.Attendee)
            .Include(r => r.Review)
                .ThenInclude(r => r.Attendee)
            .AsQueryable();

        query = ApplyFilters(parameters, query);

        query = SortingHelper.ApplySorting(query, parameters.SortOrder,
            SortingHelper.ReviewReportsSortingKeySelector(parameters.SortColumn));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
            parameters.PageIndex, parameters.PageSize, cancellationToken);

        query = PaginationHelper.ApplyPagination(query, parameters.PageIndex, parameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    private static IQueryable<ReviewReport> ApplyFilters(GetAllReviewReportsQueryParameters parameters,
        IQueryable<ReviewReport> query)
    {
        if (parameters.AttendeeId.HasValue)
        {
            query = query.Where(r => r.AttendeeId == parameters.AttendeeId);
        }
        if (parameters.Status.HasValue)
        {
            query = query.Where(r => r.Status == parameters.Status);
        }
        if (parameters.ReviewId.HasValue)
        {
            query = query.Where(r => r.ReviewId == parameters.ReviewId);
        }

        return query;
    }


    public async Task<Report?> GetReportByIdAsync(int reportId,
    CancellationToken cancellationToken)
    {
        return await context.Reports
            .Include(r => r.Attendee)
            .FirstOrDefaultAsync(r => r.Id == reportId, cancellationToken);
    }

    public async Task<EventReport?> GetEventReportByIdAsync(int reportId,
        CancellationToken cancellationToken)
    {
        return await context.EventReports
            .Include(r => r.Attendee)
            .Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == reportId, cancellationToken);
    }

    public async Task<ReviewReport?> GetReviewReportByIdAsync(int reportId,
        CancellationToken cancellationToken)
    {
        return await context.ReviewReports
            .Include(r => r.Attendee)
            .Include(r => r.Review)
            .FirstOrDefaultAsync(r => r.Id == reportId, cancellationToken);
    }
}
