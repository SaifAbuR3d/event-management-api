namespace EventManagement.Application.Exceptions;

public class UnauthenticatedException : CustomException
{
    public UnauthenticatedException(string message) : base(message)
    {
    }

    public UnauthenticatedException() : base("User is unauthenticated")
    {
    }
}
