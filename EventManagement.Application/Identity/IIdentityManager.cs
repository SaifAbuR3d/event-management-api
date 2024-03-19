﻿namespace EventManagement.Application.Identity;

public interface IIdentityManager
{
    Task<string> AuthenticateCredentials(string email, string password);
    Task<int> RegisterUser(string email, string userName, string password,
        string firstName, string lastName, string role);
}
