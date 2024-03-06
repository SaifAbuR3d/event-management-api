namespace EventManagement.Domain.Models;

public class Ticket : Entity
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
