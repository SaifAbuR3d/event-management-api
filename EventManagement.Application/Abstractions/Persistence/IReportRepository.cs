using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IReportRepository
{
    Task<int> AddEventReportAsync(EventReport report, CancellationToken cancellationToken);
    Task<int> AddReviewReportAsync(ReviewReport report, CancellationToken cancellationToken);

    Task<Report?> GetReportByIdAsync(int reportId, CancellationToken cancellationToken);
    Task<EventReport?> GetEventReportByIdAsync(int reportId, CancellationToken cancellationToken);
    Task<ReviewReport?> GetReviewReportByIdAsync(int reportId, CancellationToken cancellationToken);

    Task<(IEnumerable<EventReport>, PaginationMetadata)> GetAllEventReportsAsync(
         GetAllEventReportsQueryParameters queryParameters, CancellationToken cancellationToken);
    Task<(IEnumerable<ReviewReport>, PaginationMetadata)> GetAllReviewReportsAsync(
        GetAllReviewReportsQueryParameters queryParameters, CancellationToken cancellationToken);
}
