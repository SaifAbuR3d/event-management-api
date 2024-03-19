using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IAdminRepository
{
    Task<Admin> AddAdminAsync(Admin admin, CancellationToken cancellationToken);
    Task<Admin?> GetAdminByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Admin?> GetAdminByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<Admin?> GetAdminByEmailAsync(string email, CancellationToken cancellationToken);
}
