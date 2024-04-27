using Microsoft.AspNetCore.Http;

namespace EventManagement.Application.Abstractions.Documents;

public interface IDocumentHandler
{
    Task<string> UploadDocument(IFormFile fileData, string directory);
}

