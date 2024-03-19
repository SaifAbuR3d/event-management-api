using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Domain.Entities;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class AdminRepository(ApplicationDbContext context) 
    : IAdminRepository
{
    public async Task<Admin> AddAdminAsync(Admin admin, CancellationToken cancellationToken)
    {
        var entry = await context.Admins.AddAsync(admin, cancellationToken);
        return entry.Entity;
    }

    public Task<Admin?> GetAdminByEmailAsync(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Admin?> GetAdminByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Admin?> GetAdminByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
