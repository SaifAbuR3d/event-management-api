using MediatR;

namespace EventManagement.Application.Identity.Register;

public record RegisterAdminResponse(string Message, int AdminId);

public record RegisterAdminCommand(string Email, string Password,
    string FirstName, string LastName) : IRequest<RegisterAdminResponse>;

public class RegisterAdminCommandHandler(IIdentityManager identityManager)
    : IRequestHandler<RegisterAdminCommand, RegisterAdminResponse>
{
    public async Task<RegisterAdminResponse> Handle(RegisterAdminCommand request,
        CancellationToken cancellationToken)
    {
        var admin = await identityManager.RegisterAdmin(request.Email, request.Password,
            request.FirstName, request.LastName);

        return new RegisterAdminResponse("Registration successful", admin.Id);
    }
}
