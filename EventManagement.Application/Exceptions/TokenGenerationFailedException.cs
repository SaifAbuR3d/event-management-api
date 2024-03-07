namespace EventManagement.Application.Exceptions;

public class TokenGenerationFailedException : CustomException
{
    public TokenGenerationFailedException(string message) : base(message)
    {
    }
}
