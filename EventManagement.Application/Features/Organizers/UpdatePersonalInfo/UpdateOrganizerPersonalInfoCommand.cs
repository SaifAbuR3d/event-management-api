using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Organizers.UpdatePersonalInfo;

public record UpdateOrganizerPersonalInfoCommand(string UserName,
    string? NewFirstName, string? NewLastName, string? NewDisplayName)
    : IRequest<Unit>;

public class UpdateOrganizerPersonalInfoCommandHandler(ICurrentUser currentUser,
    IOrganizerRepository organizerRepository,
    IUserRepository userRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateOrganizerPersonalInfoCommand, Unit>
{
    public async Task<Unit> Handle(UpdateOrganizerPersonalInfoCommand request, CancellationToken cancellationToken)
    {
        if (request.NewFirstName == null && request.NewLastName == null && request.NewDisplayName == null)
        {
            throw new BadRequestException("At least one of the fields must be provided");
        }

        if (currentUser.UserName != request.UserName && !currentUser.IsAdmin)
        {
            throw new UnauthorizedException("You are not authorized to perform this action");
        }

        var organizer = await organizerRepository.GetOrganizerByUserNameAsync(request.UserName,
            cancellationToken) ?? throw new NotFoundException(nameof(Organizer),
            nameof(request.UserName), request.UserName);

        if (request.NewDisplayName != null)
            organizer.DisplayName = request.NewDisplayName;

        if (request.NewFirstName != null)
            await userRepository.UpdateFirstNameByUserName(request.UserName, request.NewFirstName,
                               cancellationToken);
        if (request.NewLastName != null)
            await userRepository.UpdateLastNameByUserName(request.UserName, request.NewLastName,
                               cancellationToken);

        // If the current user is not an admin, then organizer needs to be re-verified
        if (!currentUser.IsAdmin)
        {
            organizer.IsVerified = false;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


