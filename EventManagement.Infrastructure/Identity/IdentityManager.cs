﻿using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using Microsoft.AspNetCore.Identity;

namespace EventManagement.Infrastructure.Identity;

public class IdentityManager(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole<int>> roleManager,
                             IJwtTokenGenerator jwtTokenGenerator) : IIdentityManager
{

    public async Task<int> RegisterUser(string email, string userName, string password,
    string firstName, string lastName, string role)
    {
        var user = new ApplicationUser(firstName, lastName, email, userName);

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First().Description;
            throw new BadRequestException(error);
        }

        await AddToRole(user, role);

        return user.Id;
    }

    private async Task AddToRole(ApplicationUser user, string role)
    {
        await CreateRoleIfNotExist(role);

        var result = await userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
        {
            var error = result.Errors.First().Description;
            throw new BadRequestException(error);
        }
    }

    private async Task CreateRoleIfNotExist(string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }



    public async Task<string> AuthenticateCredentials(string email, string password)
    {
        var user = await CheckCredentials(email, password);

        var roles = await GetUserRole(user);

        var token = await jwtTokenGenerator.GenerateToken(user, roles)
            ?? throw new TokenGenerationFailedException("null token");

        return token; 
    }

    private async Task<ApplicationUser> CheckCredentials(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null || !await userManager.CheckPasswordAsync(user, password))
        {
            throw new UnauthenticatedException("Invalid Credentials");
        }

        return user;
    }

    private async Task<IList<string>> GetUserRole(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        if (roles.Count == 0)
        {
            throw new NoRolesException(user.Id);
        }

        return roles;
    }

    // used only by admin
    public async Task UpdatePassword(string username, string newPassword)
    {
        var user = await userManager.FindByNameAsync(username) 
            ?? throw new NotFoundException(name: "User", keyName: "UserName", key: username);

        // Checking if newPassword complies with the password policy 
        // the check is a MUST, because if the password does not comply with the policy, the password will be set to NULL
        var passwordValidator = userManager.PasswordValidators.First(); 
        var result = await passwordValidator.ValidateAsync(userManager, user, newPassword);

        if (!result.Succeeded)
        {
            var error = result.Errors.First().Description;
            throw new BadRequestException(error);
        }

        await userManager.RemovePasswordAsync(user);

        await userManager.AddPasswordAsync(user, newPassword);
    }

}
