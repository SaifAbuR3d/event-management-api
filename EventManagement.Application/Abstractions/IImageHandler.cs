using Microsoft.AspNetCore.Http;

namespace EventManagement.Application.Abstractions;

public interface IImageHandler
{
    Task<string> UploadImage(IFormFile file, string directory, bool isThumbnail = false);
}

