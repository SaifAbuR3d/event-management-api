namespace EventManagement.Application.Exceptions;

public class ConflictException : CustomException
{
    public ConflictException(string message) : base(message)
    { }

}
