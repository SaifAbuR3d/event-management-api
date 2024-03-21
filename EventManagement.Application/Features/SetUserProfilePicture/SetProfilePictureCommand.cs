using EventManagement.Application.Abstractions.Images;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventManagement.Application.Features.SetUserProfilePicture;

public record SetProfilePictureCommand(IFormFile Image, string BaseUrl) : IRequest<string>;

public class SetProfilePictureCommandHandler(ICurrentUser currentUser, IUserRepository userRepository,
    IUnitOfWork unitOfWork, IImageHandler imageHandler)
    : IRequestHandler<SetProfilePictureCommand, string>
{
    public async Task<string> Handle(SetProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        var usersDirectory = Path.Combine(request.BaseUrl, "images", "users", userId.ToString());
        var imageUrl = await imageHandler.UploadImage(request.Image, usersDirectory, true);

        var image = new UserImage(userId, imageUrl);
        await userRepository.AddUserImageAsync(image, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return imageUrl;
    }
}
