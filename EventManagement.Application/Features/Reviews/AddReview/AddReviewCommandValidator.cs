using FluentValidation;

namespace EventManagement.Application.Features.Reviews.AddReview;

public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0)
            .WithMessage("EventId must be greater than 0");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.Comment)
            .MaximumLength(2000)
            .WithMessage("Comment must not exceed 2000 characters");

        When(x => x.Title != null, () =>
        {
            RuleFor(x => x.Title)
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters");
        });
    }
}
