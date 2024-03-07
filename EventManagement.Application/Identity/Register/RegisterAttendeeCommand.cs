using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Models;
using MediatR;

namespace EventManagement.Application.Identity.Register;

public record RegisterAttendeeResponse(string Message, int AttendeeId);

public record RegisterAttendeeCommand(string Email, string UserName, string Password,
    string FirstName, string LastName, string Gender, DateTime DateOfBirth) : IRequest<RegisterAttendeeResponse>;

public class RegisterAttendeeCommandHandler(IIdentityManager identityManager, 
    IAttendeeRepository attendeeRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<RegisterAttendeeCommand, RegisterAttendeeResponse>
{
    public async Task<RegisterAttendeeResponse> Handle(RegisterAttendeeCommand request,
        CancellationToken cancellationToken)
    {
        var userId = await identityManager.RegisterUser(request.Email, request.UserName, 
               request.Password, request.FirstName, request.LastName, Role.Attendee.ToString());

        var attendee = new Attendee
        {
            UserId = userId,
            DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth),
            Gender = request.Gender.ToString()
        }; 

        var attendeeEntity = await attendeeRepository.AddAttendeeAsync(attendee, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterAttendeeResponse("Registration successful", attendeeEntity.Id);
    }
}

