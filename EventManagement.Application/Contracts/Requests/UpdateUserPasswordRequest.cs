using EventManagement.Application.Features.Identity.UpdateUserPassword;

namespace EventManagement.Application.Contracts.Requests;

public class UpdateUserPasswordRequest
{
    public string NewPassword { get; set; } = default!;

    public UpdateUserPasswordCommand ToCommand(string username)
    {
        return new UpdateUserPasswordCommand(username, NewPassword);
    }
}
