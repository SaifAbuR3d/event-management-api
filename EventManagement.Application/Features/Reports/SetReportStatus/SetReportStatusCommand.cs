using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Reports.SetStatusToSeen;

public record SetReportStatusCommand(int ReportId, ReportStatus NewStatus) : IRequest<Unit>;

public class SetReportStatusCommandHandler(ICurrentUser currentUser,
    IReportRepository reportRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<SetReportStatusCommand, Unit>
{
    public async Task<Unit> Handle(SetReportStatusCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAdmin)
        {
            throw new UnauthorizedAccessException("Only admins can change the status of a report");
        }

        var report = await reportRepository.GetReportByIdAsync(request.ReportId, cancellationToken)
            ?? throw new NotFoundException(nameof(Report), "Report", request.ReportId);

        if(report.Status == ReportStatus.Seen)
        {
            throw new BadRequestException("Report is already seen");
        }

        report.Seen(); 

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


