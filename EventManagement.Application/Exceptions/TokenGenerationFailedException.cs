﻿namespace EventManagement.Application.Exceptions;

public class TokenGenerationFailedException : Exception
{
    public TokenGenerationFailedException(string message) : base(message)
    {
    }
}
