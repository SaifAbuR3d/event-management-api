using EventManagement.Application.Abstractions.Persistence;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
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
}
