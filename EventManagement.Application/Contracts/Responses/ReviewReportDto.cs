using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Responses;
public class ReviewReportDto
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;
    public ReportStatus Status { get; set; } = default!;

    public int EventId { get; set; }

    public int ReviewWriterId { get; set; } // AttendeeId
    public string ReviewWriterUserName { get; set; } = default!;
    public string ReviewContent { get; set; } = default!;


    public int AttendeeId { get; set; }
    public string AttendeeName { get; set; } = default!;
    public string AttendeeUserName { get; set; } = default!;
    public string? AttendeeImageUrl { get; set; } = default!;

    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
}
