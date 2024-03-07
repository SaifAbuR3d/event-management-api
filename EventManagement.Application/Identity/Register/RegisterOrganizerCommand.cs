using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Models;
using MediatR;

namespace EventManagement.Application.Identity.Register;

public record RegisterOrganizerResponse(string Message, int OrganizerId);

public record RegisterOrganizerCommand(string Email, string UserName, string Password,
    string FirstName, string LastName, string? CompanyName) : IRequest<RegisterOrganizerResponse>;

public class RegisterOrganizerCommandHandler(IIdentityManager identityManager, 
    IOrganizerRepository organizerRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<RegisterOrganizerCommand, RegisterOrganizerResponse>
{
    public async Task<RegisterOrganizerResponse> Handle(RegisterOrganizerCommand request,
        CancellationToken cancellationToken)
    {
        var userId = await identityManager.RegisterUser(request.Email, request.UserName, 
                       request.Password, request.FirstName, request.LastName, Role.Organizer.ToString());

        var organizer = new Organizer
        {
            UserId = userId,
            CompanyName = request.CompanyName
        }; 

        var organizerEntity = await organizerRepository.AddOrganizerAsync(organizer, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterOrganizerResponse("Registration successful", organizerEntity.Id);
    }
}
