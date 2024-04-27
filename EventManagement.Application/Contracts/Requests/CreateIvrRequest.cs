using EventManagement.Application.Features.IVRs.CreateIvr;
using EventManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EventManagement.Application.Contracts.Requests;

public class CreateIvrRequest
{
    public IFormFile Document { get; set; } = default!;
    public DocumentType DocumentType { get; set; } = default!;

    public CreateIvrCommand ToCommand(string baseUrl)
    {
        return new CreateIvrCommand(Document, DocumentType, baseUrl);
    }
}
