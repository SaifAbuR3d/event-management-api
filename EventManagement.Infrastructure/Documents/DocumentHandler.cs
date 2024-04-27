using EventManagement.Application.Abstractions.Documents;
using EventManagement.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace EventManagement.Infrastructure.Documents;

public class DocumentHandler : IDocumentHandler
{
    private static readonly string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".pdf", ".word"];
    public async Task<string> UploadDocument(IFormFile fileData, string directory)
    {
        if (fileData.Length <= 0)
        {
            throw new BadFileException("File is empty");
        }

        var fileExtension = Path.GetExtension(fileData.FileName);

        if (!allowedExtensions.Contains(fileExtension.ToLower()))
        {
            throw new BadFileException("Invalid File type");
        }

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var name = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()); // random name


        var FileFullPath = Path.Combine(directory, name + fileExtension);

        if (File.Exists(FileFullPath))
        {
            File.Delete(FileFullPath);
        }

        using var stream = new FileStream(FileFullPath, FileMode.Create);
        await fileData.CopyToAsync(stream);

        return FileFullPath
               .Substring(FileFullPath.IndexOf("ivr-documents"))
               .Replace("\\", "/"); // Replace backslashes with forward slashes for URL compatibility
    }

}
