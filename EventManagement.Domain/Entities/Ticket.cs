namespace EventManagement.Domain.Entities;

public class Ticket : Entity
{
    internal Ticket() { }

    public Ticket(Event @event, string name, decimal price, int quantity, DateTime startSale, DateTime endSale)
    {
        Event = @event;
        Name = name;
        Price = price;
        TotalQuantity = quantity;
        StartSale = startSale;
        EndSale = endSale;

        AvailableQuantity = quantity;

        CreationDate = DateTime.Now;
        LastModified = DateTime.Now;
    }

    public Ticket(Event @event, string name, decimal price, int quantity, DateTime endSale)
        : this(@event, name, price, quantity, DateTime.UtcNow.AddMinutes(1), endSale)
    { }


    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int TotalQuantity { get; set; }
    public int AvailableQuantity { get; private set; }
    public DateTime StartSale { get; set; }
    public DateTime EndSale { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public ICollection<BookingTicket> BookingTickets { get; set; } = new List<BookingTicket>();

    public void DecreaseAvailableQuantity(int quantity)
    {
        if (quantity > AvailableQuantity)
        {
            throw new InvalidOperationException("Not enough tickets available");
        }

        AvailableQuantity -= quantity;
    }
}
