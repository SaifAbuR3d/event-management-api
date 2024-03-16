using EventManagement.Application.Common;
using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Models;
using FluentValidation;
using MediatR;

namespace EventManagement.Application.Identity.Register;

public record RegisterAdminCommand(string Email, string UserName, string Password,
    string FirstName, string LastName) : IRequest<RegisterAdminResponse>;

public class RegisterAdminCommandValidator : AbstractValidator<RegisterAdminCommand>
{
    public RegisterAdminCommandValidator()
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
    }
}

public class RegisterAdminCommandHandler(IIdentityManager identityManager, 
    IAdminRepository adminRepository, IUnitOfWork unitOfWork) : IRequestHandler<RegisterAdminCommand, RegisterAdminResponse>

{
    public async Task<RegisterAdminResponse> Handle(RegisterAdminCommand request,
        CancellationToken cancellationToken)
    {
        var userId = await identityManager.RegisterUser(request.Email, request.UserName, request.Password,
            request.FirstName, request.LastName, UserRole.Admin.ToString());

        var admin = new Admin
        {
            UserId = userId,
        }; 

        var adminEntity = await adminRepository.AddAdminAsync(admin, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterAdminResponse("Registration successful", adminEntity.Id);
    }
}

public record RegisterAdminResponse(string Message, int AdminId);

