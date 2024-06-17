using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Responses;
public class ReviewReportDto
{
    public int Id { get; set; }
    public string ReportContent { get; set; } = default!;
    public ReportStatus Status { get; set; } = default!;

    public int EventId { get; set; }

    public int ReviewId { get; set; }
    public int ReviewWriterId { get; set; } // AttendeeId
    public string ReviewWriterUserName { get; set; } = default!;
    public string ReviewContent { get; set; } = default!;


    public int ReportWriterId { get; set; }
    public string ReportWriterName { get; set; } = default!;
    public string ReportWriterUserName { get; set; } = default!;
    public string? ReportWriterImageUrl { get; set; } = default!;

    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
}
