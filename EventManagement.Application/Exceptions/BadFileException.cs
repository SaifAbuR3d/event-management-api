﻿namespace EventManagement.Application.Exceptions;

public class BadFileException : CustomException
{
    public BadFileException(string message) : base(message)
    { }
}
