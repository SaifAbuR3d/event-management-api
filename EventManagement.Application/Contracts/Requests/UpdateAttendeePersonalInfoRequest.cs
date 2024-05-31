using EventManagement.Application.Features.Attendees.UpdatePersonalInfo;

namespace EventManagement.Application.Contracts.Requests;

// TODO: Add Input Validation for FirstName, LastName
public class UpdateAttendeePersonalInfoRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public UpdateAttendeePersonalInfoCommand ToCommand(string username)
    {
        return new UpdateAttendeePersonalInfoCommand(username, 
            FirstName, LastName);
    }
}
