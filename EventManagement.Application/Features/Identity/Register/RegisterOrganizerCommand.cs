using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Common;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Enums;
using FluentValidation;
using MediatR;

namespace EventManagement.Application.Features.Identity.Register;

public record RegisterOrganizerCommand(string Email, string UserName, string Password,
    string FirstName, string LastName, string DisplayName) : IRequest<RegisterOrganizerResponse>;

public class RegisterOrganizerCommandValidator : AbstractValidator<RegisterOrganizerCommand>
{
    public RegisterOrganizerCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 20).WithMessage("Username must be between 3 and 20 characters");

        // password requirements which are specified in IdentityConfigurations.cs
        // are validated with Identity framework
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .ValidName();

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .ValidName();

        RuleFor(x => x.DisplayName)
            .Length(3, 50).WithMessage("Display name must be between 2 and 50 characters");
    }
}

public class RegisterOrganizerCommandHandler(IIdentityManager identityManager,
    IOrganizerRepository organizerRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterOrganizerCommand, RegisterOrganizerResponse>
{
    public async Task<RegisterOrganizerResponse> Handle(RegisterOrganizerCommand request,
        CancellationToken cancellationToken)
    {
        var userId = await identityManager.RegisterUser(request.Email, request.UserName,
                       request.Password, request.FirstName, request.LastName, UserRole.Organizer.ToString());

        var organizer = new Organizer
        {
            UserId = userId,
            DisplayName = request.DisplayName
        };

        var organizerEntity = await organizerRepository.AddOrganizerAsync(organizer, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterOrganizerResponse("Registration successful", organizerEntity.Id);
    }
}
public record RegisterOrganizerResponse(string Message, int OrganizerId);
