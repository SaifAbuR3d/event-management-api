namespace EventManagement.Domain.Entities;

public enum ReportStatus
{
    Pending,
    Seen
}

public abstract class Report : Entity
{
    internal Report() { }
    public Report(string content)
    {
        Content = content;

        Status = ReportStatus.Pending;
        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public void Seen()
    {
        if(Status == ReportStatus.Seen)
        {
            throw new InvalidOperationException("Report is already seen");
        }

        Status = ReportStatus.Seen;
        LastModified = DateTime.UtcNow;
    }

    public string Content { get; set; } = default!;
    public ReportStatus Status { get; protected set; }
    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
}
