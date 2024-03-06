namespace EventManagement.Domain.Models;

public class Booking : Entity
{
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = default!;
    public Guid AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
}
