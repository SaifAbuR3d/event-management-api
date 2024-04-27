using EventManagement.Application.Abstractions.Documents;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventManagement.Application.Features.IVRs.CreateIvr;

public record CreateIvrCommand(IFormFile Document, DocumentType DocumentType,
    string BaseUrl) : IRequest<int>;

public class CreateIvrCommandHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork,
    IDocumentHandler documentHandler, IIvrRepository ivrRepository)
    : IRequestHandler<CreateIvrCommand, int>
{
    public async Task<int> Handle(CreateIvrCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.IsAdmin)
        {
            throw new UnauthorizedException("Only other users than admins can create IVRs.");
        }

        if (await ivrRepository.HasPendingRequests(currentUser.UserId, cancellationToken))
        {
            throw new BadRequestException("You already have a pending IVR request.");
        }

        var directory = Path.Combine(request.BaseUrl, "ivr-documents");
        var documentPath = await documentHandler.UploadDocument(request.Document, directory);

        var ivr = new IdentityVerificationRequest(currentUser.UserId, documentPath, request.DocumentType);
        var entity = await ivrRepository.AddAsync(ivr, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


