using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Requests;
public class GetAllReviewReportsQueryParameters : GetAllQueryParameters
{
    public int? ReviewId { get; set; }
    public int? AttendeeId { get; set; }
    public ReportStatus? Status { get; set; }
}
