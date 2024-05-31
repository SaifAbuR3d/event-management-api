using EventManagement.Application.Features.Organizers.UpdatePersonalInfo;

namespace EventManagement.Application.Contracts.Requests;

// TODO: Add Input Validation for FirstName, LastName, DisplayName
public class UpdateOrganizerPersonalInfoRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }

    public UpdateOrganizerPersonalInfoCommand ToCommand(string username)
    {
        return new UpdateOrganizerPersonalInfoCommand(username, 
            FirstName, LastName, DisplayName);
    }
}
