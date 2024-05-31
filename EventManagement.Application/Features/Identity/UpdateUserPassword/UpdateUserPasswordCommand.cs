using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using MediatR;

namespace EventManagement.Application.Features.Identity.UpdateUserPassword;

// used only by admin, (in the admin dashboard)
public record UpdateUserPasswordCommand(string UserName, string NewPassword)
    : IRequest<Unit>;

public class UpdateUserPasswordCommandHandler(ICurrentUser currentUser,
    IIdentityManager identityManager, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserPasswordCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        if(!currentUser.IsAdmin)
        {
            throw new UnauthorizedException("You are not authorized to perform this action");
        }

        await identityManager.UpdatePassword(request.UserName, request.NewPassword); 

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

