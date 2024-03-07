using EventManagement.Application.Exceptions;
using EventManagement.Application.Identity;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace EventManagement.Infrastructure.Identity;

public class IdentityManager(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole<int>> roleManager,
                             IJwtTokenGenerator jwtTokenGenerator) : IIdentityManager
{

    public Task<string> AuthenticateCredentials(string email, string password)
    {
        throw new NotImplementedException();
    }

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

    public Task<Attendee> RegisterAttendee(string email, string password, string firstName, string lastName, string gender, DateTime dateOfBirth)
    {
        throw new NotImplementedException();
    }

    public Task<Organizer> RegisterOrganizer(string email, string password, string firstName, string lastName, string? companyName)
    {
        throw new NotImplementedException();
    }

    public Task<Admin> RegisterAdmin(string email, string password, string firstName, string lastName)
    {
        throw new NotImplementedException();
    }
}
