namespace EventManagement.Application.Exceptions;

public class InvalidStateException : CustomException
{
    public InvalidStateException(string message) : base(message)
    {
    }
}
