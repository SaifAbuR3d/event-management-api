namespace EventManagement.Domain.Entities;

public enum ReportStatus
{
    Pending,
    Seen
}

public class Report : Entity
{
    internal Report() { }

    public Report(string content, int eventId, int attendeeId)
    {
        Content = content;
        EventId = eventId;
        AttendeeId = attendeeId;

        Status = ReportStatus.Pending;
        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

    }

    public string Content { get; set; } = default!;
    public ReportStatus Status { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
}
