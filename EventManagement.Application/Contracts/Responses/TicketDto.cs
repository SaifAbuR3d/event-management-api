namespace EventManagement.Application.Contracts.Responses;

public class TicketDto
{
    /// <summary>
    /// The id of the ticket.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The name of the ticket.
    /// </summary>
    public string Name { get; set; } = default!;
    /// <summary>
    /// The price of the ticket.
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// The total quantity of the ticket.
    /// </summary>
    public int TotalQuantity { get; set; }
    /// <summary>
    /// The available quantity of the ticket.
    /// </summary>
    public int AvailableQuantity { get; set; }
    /// <summary>
    /// The quantity of sold tickets.
    /// </summary>
    public int SoldTickets => TotalQuantity - AvailableQuantity;
    /// <summary>
    /// The start date of the ticket sale.
    /// </summary>
    public DateTime StartSale { get; set; }
    /// <summary>
    /// The end date of the ticket sale.
    /// </summary>
    public DateTime EndSale { get; set; }
    /// <summary>
    /// The revenue of the ticket.
    /// </summary>
    public decimal Revenue => Price * (TotalQuantity - AvailableQuantity);
}
