using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Models;
using MediatR;

namespace EventManagement.Application.Identity.Register;

public record RegisterAdminResponse(string Message, int AdminId);

public record RegisterAdminCommand(string Email, string UserName, string Password,
    string FirstName, string LastName) : IRequest<RegisterAdminResponse>;

public class RegisterAdminCommandHandler(IIdentityManager identityManager, 
    IAdminRepository adminRepository, IUnitOfWork unitOfWork) : IRequestHandler<RegisterAdminCommand, RegisterAdminResponse>

{
    public async Task<RegisterAdminResponse> Handle(RegisterAdminCommand request,
        CancellationToken cancellationToken)
    {
        var userId = await identityManager.RegisterUser(request.Email, request.UserName, request.Password,
            request.FirstName, request.LastName, Role.Admin.ToString());

        var admin = new Admin
        {
            UserId = userId,
        }; 

        var adminEntity = await adminRepository.AddAdminAsync(admin, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterAdminResponse("Registration successful", adminEntity.Id);
    }
}
