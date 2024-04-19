using static EventManagement.Domain.Constants.Location;
using static EventManagement.Domain.Constants.CreatedTicket;
using FluentValidation;
using EventManagement.Application.Contracts.Requests;

namespace EventManagement.Application.Features.Events.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 7000).WithMessage("Title must be between 3 and 7000 characters");


        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(3, 7000).WithMessage("Description must be between 3 and 7000 characters");

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow).WithMessage("Start date must be in the future");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after than or equal to start date");

        RuleFor(x => x.StartTime)
            .NotEmpty();

        RuleFor(x => x.EndTime)
            .NotEmpty();

        // TODO - validate date time together 

        When(x => !x.IsOnline, () =>
        {
            RuleFor(h => h.Lat)
                .NotEmpty()
                .InclusiveBetween(MinLatitude, MaxLatitude);

            RuleFor(h => h.Lon)
                .NotEmpty()
                .InclusiveBetween(MinLongitude, MaxLongitude);
        });

        RuleFor(x => x.Thumbnail)
            .NotEmpty();

        When(x => x.Images != null, () =>
        {
            RuleFor(x => x.Images)
                .Must(x => x.Count <= 3).WithMessage("Maximum 3 images are allowed");
        });

        When(x => x.Tickets != null, () =>
        {
            RuleFor(x => x.Tickets)
                .Must(x => x.Count >= MinTicketTypes)
                .WithMessage($"At least {MinTicketTypes} ticket type is required")
                .Must(x => x.Count <= MaxTicketTypes)
                .WithMessage($"Maximum {MaxTicketTypes} tickets types are allowed");

            RuleForEach(x => x.Tickets)
                .SetValidator(new TicketDtoValidator());
        });

        When(x => x.IsManaged == false, () =>
        {
            RuleFor(x => x.MinAge)
                .Null().WithMessage("Minimum age is not applicable for unmanaged events");
            RuleFor(x => x.MaxAge)
                .Null().WithMessage("Maximum age is not applicable for unmanaged events");
            RuleFor(x => x.AllowedGender)
                .Null().WithMessage("Allowed gender is not applicable for unmanaged events");
        });

        When(x => x.MinAge != null, () =>
        {
            RuleFor(x => x.MinAge)
                .InclusiveBetween(18, 100).WithMessage("Minimum age must be between 18 and 100");

            RuleFor(x => x.MaxAge)
                .NotEmpty().WithMessage("Cannot set only minimum age, " +
                "you should provide both minimum age and maximum age")
                .InclusiveBetween(18, 100).WithMessage("Maximum age must be between 18 and 100")
                .GreaterThanOrEqualTo(x => x.MinAge).WithMessage("Maximum age must be greater than or equal to minimum age");
        });

        When(x => x.AllowedGender != null, () =>
        {
            RuleFor(x => x.AllowedGender)
                .IsInEnum().WithMessage("Allowed gender should be either Male or Female");
        });

    }
}

public class TicketDtoValidator : AbstractValidator<CreateTicketRequest>
{
    public TicketDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 50).WithMessage("Ticket name must be between 3 and 50 characters");

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThanOrEqualTo(MinPrice).WithMessage($"Price must be greater than or equal to {MinPrice}")
            .LessThanOrEqualTo(MaxPrice).WithMessage($"Price must be less than or equal to {MaxPrice}");

        RuleFor(x => x.TotalQuantity)
            .NotEmpty()
            .GreaterThanOrEqualTo(MinQuantity).WithMessage($"Total quantity must be greater than or equal to {MinQuantity}")
            .LessThanOrEqualTo(MaxQuantity)
            .WithMessage($"Total quantity must be less than or equal to {MaxQuantity}");


        RuleFor(x => x.StartSale)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow).WithMessage("Start sale date must be in the future");

        RuleFor(x => x.EndSale)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartSale)
            .WithMessage("End sale date must be after than or equal to start sale date");
    }
}
