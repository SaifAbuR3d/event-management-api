using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.RegistrationRequests.SetRegRequestStatus;

public record SetRegRequestCommand(int EventId, int RegRequestId, RegistrationStatus NewStatus) 
    : IRequest<Unit>;

public class SetRegRequestCommandHandler(ICurrentUser currentUser,
    IRegRequestRepository regRequestRepository,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SetRegRequestCommand, Unit>
{
    public async Task<Unit> Handle(SetRegRequestCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsOrganizer)
        {
            throw new UnauthorizedException("Only organizers can change the status" +
                " of a registration request");
        }

        var regRequest = await regRequestRepository.GetByIdAsync(request.RegRequestId,
            cancellationToken)
            ?? throw new NotFoundException(nameof(RegistrationRequest), request.RegRequestId);

        if(regRequest.EventId != request.EventId)
        {
            throw new UnauthorizedException("Registration request does not belong to the event");
        }

        var @event = await eventRepository.GetEventByIdAsync(regRequest.EventId,
            cancellationToken)
            ?? throw new NotFoundException(nameof(Event), regRequest.EventId);

        Console.WriteLine(currentUser.UserId);
        if(@event.Organizer.UserId != currentUser.UserId)
        {
            throw new UnauthorizedException("You are not authorized to change the status" +
                " of this registration request, you are not the organizer of the event");
        }

        if(regRequest.Status != RegistrationStatus.Pending)
        {
            throw new BadRequestException("Registration request is already processed");
        }

        else if (request.NewStatus == RegistrationStatus.Approved)
        {
            regRequest.Approve(); 
        }
        else if (request.NewStatus == RegistrationStatus.Rejected)
        {
            regRequest.Reject(); 
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
