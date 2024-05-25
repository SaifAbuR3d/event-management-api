using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IReportRepository
{
    Task<int> AddReportAsync(Report report, CancellationToken cancellationToken);
    Task<Report?> GetReportByIdAsync(int reportId, CancellationToken cancellationToken);
    Task<(IEnumerable<Report>, PaginationMetadata)> GetAllReportsAsync(
        GetAllReportsQueryParameters queryParameters, CancellationToken cancellationToken);
    Task<(IEnumerable<Report>, PaginationMetadata)> GetReportsByEventIdAsync(int eventId,
        GetAllQueryParameters queryParameters, CancellationToken cancellationToken);
    Task<(IEnumerable<Report>, PaginationMetadata)> GetReportsByAttendeeIdAsync(int attendeeId,
        GetAllQueryParameters queryParameters, CancellationToken cancellationToken);
}
