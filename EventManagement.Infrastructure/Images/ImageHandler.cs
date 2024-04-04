using EventManagement.Application.Abstractions.Images;
using EventManagement.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace EventManagement.Infrastructure.Images;

public class ImageHandler : IImageHandler
{
    private static readonly string[] allowedExtensions = [".jpg", ".jpeg", ".png"];

    public async Task<string> UploadImage(IFormFile imageData, string directory, bool thumbnail = false)
    {
        if (imageData.Length <= 0)
        {
            throw new BadFileException("Image is empty");
        }

        var imageExtension = Path.GetExtension(imageData.FileName);

        if (!allowedExtensions.Contains(imageExtension.ToLower()))
        {
            throw new BadFileException("Invalid image type");
        }

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var name = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()); // random name

        if (thumbnail)
        {
            name = "thumbnail";
        }

        var imageFullPath = Path.Combine(directory, name + imageExtension);

        if (File.Exists(imageFullPath))
        {
            File.Delete(imageFullPath);
        }

        using var stream = new FileStream(imageFullPath, FileMode.Create);
        await imageData.CopyToAsync(stream);

        return imageFullPath
               .Substring(imageFullPath.IndexOf("images"))
               .Replace("\\", "/"); // Replace backslashes with forward slashes for URL compatibility
    }
    public async Task<string> UploadQrCodeImageAsync(byte[] imageData, string directory,
        string imageName, string imageExtension, CancellationToken cancellationToken)
    {
        if (imageData.Length <= 0)
        {
            throw new BadFileException("Image is empty");
        }

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var imageFullPath = Path.Combine(directory, imageName + imageExtension);

        if (File.Exists(imageFullPath))
        {
            File.Delete(imageFullPath);
        }

        Image<Rgba32> image = Image.Load<Rgba32>(imageData);


        using var stream = new FileStream(imageFullPath, FileMode.Create);
        await image.SaveAsPngAsync(stream, cancellationToken);

        return imageFullPath
               .Substring(imageFullPath.IndexOf("images"))
               .Replace("\\", "/"); // Replace backslashes with forward slashes for URL compatibility
    }
}
