using MediatR;

namespace EventManagement.Application.Identity.Register;

public record RegisterAttendeeResponse(string Message, int AttendeeId);

public record RegisterAttendeeCommand(string Email, string Password,
    string FirstName, string LastName, string Gender, DateTime DateOfBirth) : IRequest<RegisterAttendeeResponse>;

public class RegisterAttendeeCommandHandler(IIdentityManager identityManager) 
    : IRequestHandler<RegisterAttendeeCommand, RegisterAttendeeResponse>
{
    public async Task<RegisterAttendeeResponse> Handle(RegisterAttendeeCommand request,
        CancellationToken cancellationToken)
    {
        var attendee = await identityManager.RegisterAttendee(request.Email, request.Password, request.FirstName, 
            request.LastName, request.Gender, request.DateOfBirth);

        return new RegisterAttendeeResponse("Registration successful", attendee.Id);
    }
}

