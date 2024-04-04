namespace EventManagement.Domain.Entities;

public class Booking : Entity
{
    internal Booking()
    { }

    public Booking(Attendee attendee, Event @event, ICollection<BookingTicket> bookingTickets)
    {
        Attendee = attendee;
        Event = @event;
        BookingTickets = bookingTickets;

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }


    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public ICollection<BookingTicket> BookingTickets { get; set; } = new List<BookingTicket>();
}
