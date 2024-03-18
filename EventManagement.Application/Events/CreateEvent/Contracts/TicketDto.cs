namespace EventManagement.Application.Events.CreateEvent.Contracts;

/// <summary>
/// The request to create a new ticket.
/// </summary>
public class TicketDto
{
    /// <summary>
    /// The name of the ticket.
    /// </summary>
    public string Name { get; set; } = default!;
    /// <summary>
    /// The price of the ticket.
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// The quantity of the ticket.
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// The start date of the ticket sale.
    /// </summary>
    public DateTime StartSale { get; set; }
    /// <summary>
    /// The end date of the ticket sale.
    /// </summary>
    public DateTime EndSale { get; set; }
}
