namespace EventManagement.Application.Contracts.Responses;

public class EventStatisticsDto(IEnumerable<TicketDto> tickets,
    IEnumerable<AttendeeTransaction> attendeesRecentlyBought,
    IEnumerable<SellingRecord> sellingTrack, 
    bool isManaged)
{
    public bool IsManaged => isManaged;
    public IEnumerable<TicketDto> Tickets { get; set; } = tickets;
    public int TotalTicketsQuantity => Tickets.Sum(t => t.TotalQuantity);
    public int TotalTicketsSold => Tickets.Sum(t => t.SoldTickets);
    public int TotalTicketsAvailable => Tickets.Sum(t => t.AvailableQuantity);
    public decimal TotalRevenue => Tickets.Sum(t => t.Revenue);

    public IEnumerable<AttendeeTransaction> AttendeesRecentlyBought { get; set; } = attendeesRecentlyBought;


    // in the last 14 day, how much tickets did we sell
    public IEnumerable<SellingRecord>  SellingTrack { get; set; } = sellingTrack;
}

// simple DTO to represent date and number of tickets sold
public record SellingRecord(DateTime Date, int SoldTickets);

// simple DTO to represent a transaction of an attendee
public record AttendeeTransaction(int AttendeeId, string UserName, string FullName,
    string? ImageUrl, int TicketsBought, DateTime TransactionDate);