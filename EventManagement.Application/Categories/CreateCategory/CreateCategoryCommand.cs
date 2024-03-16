using EventManagement.Application.Common;
using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Models;
using FluentValidation;
using MediatR;

namespace EventManagement.Application.Categories.CreateCategory;

public record CreateCategoryCommand(string Name, string? Description) : IRequest<CreateCategoryResponse>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .ValidName(2, 60);

        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .Length(3, 200).WithMessage("Description must be between 3 and 100 characters");
        }); 

    }
}

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var newCategory = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        var addedCategory = await categoryRepository.AddCategoryAsync(newCategory, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateCategoryResponse(addedCategory.Id, addedCategory.Name);
    }
}

public record CreateCategoryResponse(int CategoryId, string CategoryName);
