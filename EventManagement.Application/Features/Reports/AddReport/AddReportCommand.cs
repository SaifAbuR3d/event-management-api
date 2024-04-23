using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Reports.AddReport;

public record AddReportCommand(string Content, int EventId) : IRequest<int>;

public class AddReportCommandHandler(ICurrentUser currentUser,
    IReportRepository reportRepository, IEventRepository eventRepository, 
    IAttendeeRepository attendeeRepository, 
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddReportCommand, int>
{

    public async Task<int> Handle(AddReportCommand request, CancellationToken cancellationToken)
    {
        if(!currentUser.IsAttendee)
        {
            throw new UnauthorizedException("Only attendees can add reports");
        }

        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        var attendee = await attendeeRepository.GetAttendeeByUserIdAsync(
            currentUser.UserId, cancellationToken) 
            ?? throw new NotFoundException(nameof(Attendee), currentUser.UserId);

        var report = new Report(request.Content, request.EventId, attendee.Id);

        await reportRepository.AddReportAsync(report, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return report.Id;
    }
}