namespace EventManagement.Domain.Models;

public class Ticket : Entity
{
    internal Ticket() { }

    public Ticket(Event @event, string name, decimal price, int quantity, DateTime startSale, DateTime endSale)
    {
        Event = @event;
        Name = name;
        Price = price;
        Quantity = quantity;
        StartSale = startSale;
        EndSale = endSale;

        CreationDate = DateTime.Now;
        LastModified = DateTime.Now;
    }

    public Ticket(Event @event, string name, decimal price, int quantity, DateTime endSale)
        : this(@event, name, price, quantity, DateTime.UtcNow.AddMinutes(1), endSale)
    { }


    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime StartSale { get; set; }
    public DateTime EndSale { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
