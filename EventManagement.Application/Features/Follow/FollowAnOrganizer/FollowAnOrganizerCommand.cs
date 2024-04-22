using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Follow.FollowAnOrganizer;

public record FollowAnOrganizerCommand(int OrganizerId) : IRequest<Unit>;

public class FollowAnOrganizerCommandHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork,
    IAttendeeRepository attendeeRepository, IOrganizerRepository organizerRepository)
    : IRequestHandler<FollowAnOrganizerCommand, Unit>
{
    public async Task<Unit> Handle(FollowAnOrganizerCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAttendee)
        {
            throw new UnauthorizedException("Only attendees can follow organizers");
        }
        var attendee = await attendeeRepository.GetAttendeeByUserIdAsync(currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), currentUser.UserId);

        var organizer = await organizerRepository.GetOrganizerByIdAsync(request.OrganizerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Organizer), request.OrganizerId);

        var isFollowing = await attendeeRepository.IsFollowingOrganizer(attendee.Id, organizer.Id, cancellationToken);

        if (isFollowing)
        {
            throw new BadRequestException("Attendee is already following this organizer");
        }

        attendee.Follow(organizer);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
