using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using FluentValidation;
using MediatR;

namespace EventManagement.Application.Features.Identity.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}

public class LoginCommandHandler(IIdentityManager identityManager)
    : IRequestHandler<LoginCommand, LoginResponse>
{

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var token = await identityManager.AuthenticateCredentials(request.Email, request.Password)
            ?? throw new UnauthenticatedException("Invalid Credentials");

        return new LoginResponse(token);
    }
}

public record LoginResponse(string Token); // token contains userId, email and role
