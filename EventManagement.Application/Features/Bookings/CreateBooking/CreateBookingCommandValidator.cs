using EventManagement.Application.Contracts.Requests;
using FluentValidation;
using static EventManagement.Domain.Constants.RequestedTicket; 

namespace EventManagement.Application.Features.Bookings.CreateBooking;

public class RequestedTicketValidator : AbstractValidator<RequestedTicket>
{
    public RequestedTicketValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("TicketId must be greater than 0."); 

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .GreaterThanOrEqualTo(MinQuantity).WithMessage($"Quantity must be greater than or equal to {MinQuantity}.")
            .LessThanOrEqualTo(MaxQuantity).WithMessage($"Quantity must be less than or equal " +
            $"to {MaxQuantity}.");
    }
}

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("EventId must be greater than 0.");

        RuleFor(x => x.Tickets)
            .NotEmpty()
            .WithMessage("Tickets must not be empty.");

        RuleForEach(x => x.Tickets)
            .SetValidator(new RequestedTicketValidator());

        RuleFor(x => x.PaymentMethodId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("PaymentMethodId must be greater than 0.");
    }
}

