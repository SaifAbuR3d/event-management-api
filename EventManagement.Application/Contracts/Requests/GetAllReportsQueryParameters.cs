using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Requests;

public class GetAllReportsQueryParameters : GetAllQueryParameters
{
    public string? Content { get; set; }
    public int? EventId { get; set; }
    public int? OrganizerId { get; set; }
    public int? AttendeeId { get; set; }
    public ReportStatus? Status { get; set; }
}
