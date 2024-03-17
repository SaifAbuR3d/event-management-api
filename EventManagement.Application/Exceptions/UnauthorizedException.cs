namespace EventManagement.Application.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string userId, string role) :
        base($"The user with Id {userId} has no role {role}")
    {
    }

    public UnauthorizedException(string userId, string entityName, object entityId) :
        base($"The user with Id {userId} has no access to Entity '{entityName}' ({entityId})")
    {
    }
}
