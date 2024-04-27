using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.IVRs.SetIvrStatus;
public record RejectIvrCommand(int Id, string? AdminMessage)
    : IRequest<Unit>;

public class RejectIvrCommandHandler(ICurrentUser currentUser,
    IUnitOfWork unitOfWork, IIvrRepository ivrRepository)
    : IRequestHandler<RejectIvrCommand, Unit>
{
    public async Task<Unit> Handle(RejectIvrCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAdmin)
        {
            throw new UnauthorizedException("Only admins can reject IVR requests.");
        }

        var ivr = await ivrRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(IdentityVerificationRequest), request.Id);

        if (ivr.Status != IdentityVerificationRequestStatus.Pending)
        {
            throw new BadRequestException("IVR request is not pending.");
        }

        ivr.Reject(request.AdminMessage);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}