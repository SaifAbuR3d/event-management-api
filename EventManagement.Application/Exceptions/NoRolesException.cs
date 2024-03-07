namespace EventManagement.Application.Exceptions;

public class NoRolesException : CustomException
{
    public NoRolesException(int userId) : base($"No Roles For the user with Id {userId}")
    {
    }
}
