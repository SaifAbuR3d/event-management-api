using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Responses;

public class PersonTicketDto
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public Guid CheckInCode { get; set; }
    public string? HolderName { get; set; }
    public DateTime CreationDate { get; set; }

    public string QrCodeImageUrl { get; set; } = default!;

    public string TicketTypeName { get; set; } = default!;
    public int TicketTypeId { get; set; } = default!;

    public int BookingId { get; set; }
}
