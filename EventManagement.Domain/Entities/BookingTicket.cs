namespace EventManagement.Domain.Entities;

public class BookingTicket : Entity
{
    internal BookingTicket()
    { }

    public BookingTicket(Ticket ticket, string qrCodeImageUrl, Guid checkInCode)
    {
        Ticket = ticket;
        CheckInCode = checkInCode;

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        QrCodeImageUrl = qrCodeImageUrl;
    }

    public BookingTicket(Ticket ticket, string holderName, string qrCodeImageUrl, Guid checkInCode)
        : this(ticket, qrCodeImageUrl, checkInCode)
    {
        HolderName = holderName;
    }

    public void CheckIn()
    {
        if(IsCheckedIn)
        {
            throw new InvalidOperationException("Ticket is already checked in");
        }

        IsCheckedIn = true;
    }

    public string? HolderName { get; set; }
    public string QrCodeImageUrl { get; set; } = default!;

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = default!;
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = default!;
    public Guid CheckInCode { get; set; }
    public bool IsCheckedIn { get; set; }
}
