using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Responses;

public class EventReportDto
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;
    public ReportStatus Status { get; set; } = default!;
    public int EventId { get; set; }
    public string EventName { get; set; } = default!;
    public int OrganizerId { get; set; }
    public string OrganizerUserName { get; set; } = default!;

    public int AttendeeId { get; set; }
    public string AttendeeName { get; set; } = default!;
    public string AttendeeUserName { get; set; } = default!;
    public string? AttendeeImageUrl { get; set; } = default!;
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
}
