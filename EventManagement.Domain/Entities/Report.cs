namespace EventManagement.Domain.Entities;

public enum ReportStatus
{
    Pending,
    Seen
}

public class Report : Entity
{
    public string Content { get; set; } = default!;
    public ReportStatus Status { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
}
