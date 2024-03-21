using EventManagement.Application.Common;
using static EventManagement.Domain.Constants.Location;
using FluentValidation;
using EventManagement.Application.Contracts.Responses;

namespace EventManagement.Application.Features.Events.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .ValidName();

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(3, 200).WithMessage("Description must be between 3 and 200 characters");

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
            .NotEmpty()
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

        When(x => !x.IsOnline, () =>
        {
            RuleFor(h => h.Lat)
                .NotEmpty()
                .InclusiveBetween(MinLatitude, MaxLatitude);

            RuleFor(h => h.Lon)
                .NotEmpty()
                .InclusiveBetween(MinLongitude, MaxLongitude);

            RuleFor(x => x.Street)
                .NotEmpty();

            RuleFor(x => x.CityId)
                .NotEmpty();
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
                .Must(x => x.Count > 0).WithMessage("At least one ticket type is required")
                .Must(x => x.Count <= 5).WithMessage("Maximum 5 tickets types are allowed");

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

public class TicketDtoValidator : AbstractValidator<TicketDto>
{
    public TicketDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 50).WithMessage("Ticket name must be between 3 and 50 characters");

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.StartSale)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow).WithMessage("Start sale date must be in the future");

        RuleFor(x => x.EndSale)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartSale).WithMessage("End sale date must be after than or equal to start sale date");
    }
}
