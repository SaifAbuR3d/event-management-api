using EventManagement.Application.Exceptions;
using MediatR;

namespace EventManagement.Application.Identity.Login;

public record LoginResponse(string Token); // token contains userId, email and role

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

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