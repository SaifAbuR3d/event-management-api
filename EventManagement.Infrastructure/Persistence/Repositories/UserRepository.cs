using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<UserImage?> AddUserImageAsync(UserImage userImage, CancellationToken cancellationToken)
    {
        if (await GetProfilePictureByUserId(userImage.UserId, cancellationToken) != null)
        {
            await DeleteProfilePictureByUserId(userImage.UserId, cancellationToken);
        }
        var entry = await context.UserImages.AddAsync(userImage, cancellationToken);
        return entry.Entity;
    }

    public async Task<string?> GetEmailByUserId(int userId, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync(userId, cancellationToken);
        return user?.Email;
    }

    public async Task<string?> GetFullNameByUserId(int userId, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync(userId, cancellationToken);
        if (user == null || user.FirstName == null || user.LastName == null)
        {
            return null;
        }
        return $"{user.FirstName} {user.LastName}";
    }

    public async Task<string?> GetUserNameByUserId(int userId, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync(userId, cancellationToken);
        return user?.UserName;
    }

    public async Task<string?> GetProfilePictureByUserId(int userId, CancellationToken cancellationToken)
    {
        var user = await context.UserImages.FirstOrDefaultAsync(ui => ui.UserId == userId, cancellationToken); 
        return user?.ImageUrl;
    }

    public async Task<string?> GetIdByUserId(int userId, CancellationToken cancellationToken)
    {
        bool attendeeExists = await context.Attendees
            .AnyAsync(a => a.UserId == userId, cancellationToken);
        if (attendeeExists)
        {
            return await context.Attendees
                .Where(a => a.UserId == userId)
                .Select(a => a.Id.ToString())
                .FirstOrDefaultAsync(cancellationToken);
        }

        bool organizerExists = await context.Organizers
            .AnyAsync(o => o.UserId == userId, cancellationToken);
        if (organizerExists)
        {
            return await context.Organizers
                .Where(o => o.UserId == userId)
                .Select(o => o.Id.ToString())
                .FirstOrDefaultAsync(cancellationToken);
        }

        return context.Admins
            .Where(a => a.UserId == userId)
            .Select(a => a.Id.ToString())
            .FirstOrDefault();
    }

    public async Task DeleteProfilePictureByUserId(int userId, CancellationToken cancellationToken)
    {
        var userImage = await context.UserImages.FirstOrDefaultAsync(ui => ui.UserId == userId, cancellationToken);
        if (userImage != null)
        {
            context.UserImages.Remove(userImage);
        }
    }

    public async Task<bool> IsVerified(int userId, CancellationToken cancellationToken)
    {
        return await context.IdentityVerificationRequests
            .AnyAsync(ivr => ivr.UserId == userId
                          && ivr.Status == IdentityVerificationRequestStatus.Approved
                          , cancellationToken); 
    }

    public async Task<string?> GetEmailByUserName(string userName, CancellationToken cancellationToken)
    {
        return await context.Users
            .Where(u => u.UserName == userName)
            .Select(u => u.Email)
            .FirstOrDefaultAsync(cancellationToken); 
    }

    public async Task UpdateFirstNameByUserName(string userName, string newFirstName,
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName,cancellationToken);

        if (user == null)
        {
            return;
        }

        user.FirstName = newFirstName;
    }

    public async Task UpdateLastNameByUserName(string userName, string newLastName,
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        
        if (user == null)
        {
            return;
        }

        user.LastName = newLastName;
    }
}
