namespace EventManagement.Domain.Entities;
public class EventReport : Report
{
    internal EventReport() { }
    public EventReport(string content, int eventId, int attendeeId)
        : base(content)
    {
        Content = content;
        EventId = eventId;
        AttendeeId = attendeeId;
    }
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
}
