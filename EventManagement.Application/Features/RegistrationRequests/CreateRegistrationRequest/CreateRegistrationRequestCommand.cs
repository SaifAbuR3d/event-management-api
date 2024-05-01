using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.RegistrationRequests.CreateRegistrationRequest;

public record CreateRegistrationRequestCommand(int EventId) : IRequest<Unit>;

public class CreateRegistrationRequestCommandHandler(ICurrentUser currentUser,
    IUserRepository userRepository,
    IAttendeeRepository attendeeRepository,
    IEventRepository eventRepository,
    IRegRequestRepository regRequestRepository, 
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateRegistrationRequestCommand, Unit>
{
    public async Task<Unit> Handle(CreateRegistrationRequestCommand request, CancellationToken cancellationToken)
    {
        if(!currentUser.IsAttendee)
        {
            throw new UnauthorizedException("Only attendees can create registration requests.");
        }

        var attendee = await attendeeRepository.GetAttendeeByUserIdAsync(currentUser.UserId,cancellationToken)
            ?? throw new NotFoundException(nameof(Attendee), "UserId", currentUser.UserId);

        bool isVerified = await userRepository.IsVerified(attendee.Id, cancellationToken);

        //if (!isVerified)
        //{
        //    throw new UnauthorizedException("You must verify your identity before you can make" +
        //        " a registration request.");
        //}

        bool hasMadeRegRequest = await attendeeRepository.HasMadeRegRequest(attendee.Id,
            request.EventId, cancellationToken);

        if(hasMadeRegRequest)
        {
            throw new ConflictException("You have already made a registration request for this event.");
        }

        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        //if(!@event.TicketsSalesRunning())
        //{
        //    throw new BadRequestException("Registration for this event has closed.");
        //}

        var regRequest = new RegistrationRequest(attendee, @event);

        await regRequestRepository.AddAsync(regRequest, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
