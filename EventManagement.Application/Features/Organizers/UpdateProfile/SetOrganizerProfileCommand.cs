using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Features.Identity;
using EventManagement.Application.Exceptions;
using MediatR;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Features.Organizers.UpdateProfile;

// For Add and Update commands, we use the same command class
// TODO: Add validation for the properties
public record SetOrganizerProfileCommand(string? Bio,
    string? Website, string? LinkedIn, string? Instagram, string? Facebook, string? Twitter) : IRequest<Unit>;

public class UpdateOrganizerProfileCommandHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork,
    IOrganizerRepository organizerRepository)
    : IRequestHandler<SetOrganizerProfileCommand, Unit>
{
    public async Task<Unit> Handle(SetOrganizerProfileCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsOrganizer)
            throw new UnauthorizedException("Only organizers can update their profile");

        if (string.IsNullOrWhiteSpace(request.Bio) && string.IsNullOrWhiteSpace(request.Website)
            && string.IsNullOrWhiteSpace(request.LinkedIn) && string.IsNullOrWhiteSpace(request.Instagram)
            && string.IsNullOrWhiteSpace(request.Facebook) && string.IsNullOrWhiteSpace(request.Twitter))
            throw new BadRequestException("At least one field must be updated");


        var organizer = await organizerRepository.GetOrganizerByUserIdAsync(currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Organizer), nameof(Organizer.UserId), currentUser.UserId);

        organizer.SetProfile(request.Bio, request.Website,
            request.LinkedIn, request.Instagram, request.Facebook, request.Twitter);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}