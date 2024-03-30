using EventManagement.Application.Features.Bookings.CreateBooking;

namespace EventManagement.Application.Contracts.Requests;


public class CreateBookingRequest
{
    /// <summary>
    /// Converts the <see cref="CreateBookingRequest"/> object to a <see cref="CreateBookingCommand"/> object.
    /// </summary>
    /// <returns>The created <see cref="CreateBookingCommand"/> object.</returns>
    public CreateBookingCommand ToCommand(int eventId)
    {
        return new CreateBookingCommand();
    }

    public class RequestedTicket
    {
        public int TicketId { get; set; }
        public int Quantity { get; set; }
    }

    public List<RequestedTicket> Tickets { get; set; } = [];
    public string? Notes { get; set; }
    public int? PaymentMethodId { get; set; }
}
