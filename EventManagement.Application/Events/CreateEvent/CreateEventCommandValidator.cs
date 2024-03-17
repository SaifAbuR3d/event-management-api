using EventManagement.Application.Common;
using static EventManagement.Domain.Constants.Location;
using FluentValidation;

namespace EventManagement.Application.Events.CreateEvent;

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
            .GreaterThan(DateTime.Now).WithMessage("Start date must be in the future");

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
    }
}
