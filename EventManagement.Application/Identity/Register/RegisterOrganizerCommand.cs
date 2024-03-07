using MediatR;

namespace EventManagement.Application.Identity.Register;

public record RegisterOrganizerResponse(string Message, int OrganizerId);

public record RegisterOrganizerCommand(string Email, string Password,
    string FirstName, string LastName, string? CompanyName) : IRequest<RegisterOrganizerResponse>;

public class RegisterOrganizerCommandHandler(IIdentityManager identityManager) 
    : IRequestHandler<RegisterOrganizerCommand, RegisterOrganizerResponse>
{
    public async Task<RegisterOrganizerResponse> Handle(RegisterOrganizerCommand request,
        CancellationToken cancellationToken)
    {
        var organizer = await identityManager.RegisterOrganizer(request.Email, request.Password, request.FirstName, 
                       request.LastName, request.CompanyName);

        return new RegisterOrganizerResponse("Registration successful", organizer.Id);
    }
}
