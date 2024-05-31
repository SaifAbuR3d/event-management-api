using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<string?> GetFullNameByUserId(int userId, CancellationToken cancellationToken);
    Task<string?> GetUserNameByUserId(int userId, CancellationToken cancellationToken);
    Task<string?> GetEmailByUserId(int userId, CancellationToken cancellationToken);
    Task<string?> GetProfilePictureByUserId(int userId, CancellationToken cancellationToken);
    Task<UserImage?> AddUserImageAsync(UserImage userImage, CancellationToken cancellationToken);
    Task DeleteProfilePictureByUserId(int userId, CancellationToken cancellationToken);
    Task<string?> GetIdByUserId(int userId, CancellationToken cancellationToken);
    Task<bool> IsVerified(int userId, CancellationToken cancellationToken);
    Task<string?> GetEmailByUserName(string userName, CancellationToken cancellationToken);
    Task UpdateFirstNameByUserName(string userName, string newFirstName, CancellationToken cancellationToken);
    Task UpdateLastNameByUserName(string userName, string newLastName, CancellationToken cancellationToken);
}
