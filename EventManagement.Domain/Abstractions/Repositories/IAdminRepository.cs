using EventManagement.Domain.Models;

namespace EventManagement.Domain.Abstractions.Repositories;

public interface IAdminRepository
{
    Task<Admin> AddAdminAsync(Admin admin, CancellationToken cancellationToken);
    Task<Admin?> GetAdminByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Admin?> GetAdminByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<Admin?> GetAdminByEmailAsync(string email, CancellationToken cancellationToken);
}
