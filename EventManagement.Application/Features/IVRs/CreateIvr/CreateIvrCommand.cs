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

        var userId = currentUser.UserId;

        var vr = await ivrRepository.GetByUserIdAsync(userId, cancellationToken);

        if(vr?.Status == IdentityVerificationRequestStatus.Pending)
        {
            throw new ConflictException("You already have a pending IVR request.");
        }
        if(vr?.Status == IdentityVerificationRequestStatus.Approved)
        {
            throw new ConflictException("You are already verified.");
        }
        if(vr?.Status == IdentityVerificationRequestStatus.Rejected)
        {
            await ivrRepository.DeleteByUserId(userId, cancellationToken);
        }

        var directory = Path.Combine(request.BaseUrl, "ivr-documents");
        var documentPath = await documentHandler.UploadDocument(request.Document, directory);

        var ivr = new IdentityVerificationRequest(userId, documentPath, request.DocumentType)
        {
            IsForOrganizer = currentUser.IsOrganizer
        };

        var entity = await ivrRepository.AddAsync(ivr, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


