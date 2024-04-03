namespace EventManagement.Domain.Entities;

public class BookingTicket : Entity
{
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = default!;
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = default!;

    public string? HolderName { get; set; }
}
