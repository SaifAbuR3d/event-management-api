﻿using Microsoft.AspNetCore.Http;

namespace EventManagement.Application.Abstractions.Images;

public interface IImageHandler
{
    Task<string> UploadImage(IFormFile file, string directory, bool isThumbnail = false);
    Task<string> UploadQrCodeImageAsync(byte[] imageData, string directory, string imageName, string imageExtension, 
        CancellationToken cancellationToken);
}

