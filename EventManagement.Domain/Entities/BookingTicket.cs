namespace EventManagement.Domain.Entities;

public class BookingTicket : Entity
{
    internal BookingTicket()
    { }

    public BookingTicket(Ticket ticket)
    {
        Ticket = ticket;
        CheckInCode = Guid.NewGuid();

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public BookingTicket(Ticket ticket, string holderName)
        : this(ticket)
    {
        HolderName = holderName;
    }

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = default!;
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = default!;
    public Guid CheckInCode { get; set; }
    public string? HolderName { get; set; }
}
