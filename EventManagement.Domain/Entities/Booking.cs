namespace EventManagement.Domain.Entities;

public class Booking : Entity
{
    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public ICollection<BookingTicket> BookingTickets { get; set; } = new List<BookingTicket>();
}
