using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Attendees.UpdatePersonalInfo;

public record UpdateAttendeePersonalInfoCommand(string UserName,
    string? NewFirstName, string? NewLastName)
    : IRequest<Unit>;

public class UpdateAttendeePersonalInfoCommandHandler(ICurrentUser currentUser,
    IAttendeeRepository attendeeRepository,
    IUserRepository userRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateAttendeePersonalInfoCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAttendeePersonalInfoCommand request, CancellationToken cancellationToken)
    {
        if (request.NewFirstName == null && request.NewLastName == null)
        {
            throw new BadRequestException("At least one of the fields must be provided");
        }

        if (currentUser.UserName != request.UserName && !currentUser.IsAdmin)
        {
            throw new UnauthorizedException("You are not authorized to perform this action");
        }

        var attendee = await attendeeRepository.GetAttendeeByUserNameAsync(request.UserName,
            cancellationToken) ?? throw new NotFoundException(nameof(Attendee),
            nameof(request.UserName), request.UserName);

        if (request.NewFirstName != null)
            await userRepository.UpdateFirstNameByUserName(request.UserName, request.NewFirstName,
                               cancellationToken);
        if (request.NewLastName != null)
            await userRepository.UpdateLastNameByUserName(request.UserName, request.NewLastName,
                               cancellationToken);

        // If the current user is not an admin, then organizer needs to be re-verified
        if (!currentUser.IsAdmin)
        {
            attendee.IsVerified = false;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


