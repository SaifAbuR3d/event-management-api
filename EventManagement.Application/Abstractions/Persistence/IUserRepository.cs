﻿using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<string?> GetFullNameByUserId(int userId, CancellationToken cancellationToken);
    Task<string?> GetUserNameByUserId(int userId, CancellationToken cancellationToken);
    Task<string?> GetEmailByUserId(int userId, CancellationToken cancellationToken);
    Task<string?> GetProfilePictureByUserId(int userId, CancellationToken cancellationToken);
    Task<UserImage?> AddUserImageAsync(UserImage userImage, CancellationToken cancellationToken);
}