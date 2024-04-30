using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using MediatR;

namespace EventManagement.Application.Features.Reports.GetReports;

public record GetAllReportsQuery(GetAllQueryParameters Parameters)
    : IRequest<(IEnumerable<ReportDto> Reports, PaginationMetadata PaginationMetadata)>;

public class GetAllReportsQueryHandler(ICurrentUser currentUser, IReportRepository reportRepository
    , IUserRepository userRepository)
    : IRequestHandler<GetAllReportsQuery, (IEnumerable<ReportDto> Reports, PaginationMetadata PaginationMetadata)>
{

    public async Task<(IEnumerable<ReportDto> Reports, PaginationMetadata PaginationMetadata)> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
    {
        if(!currentUser.IsAdmin)
        {
            throw new UnauthorizedException("Only admins can view reports");
        }

        var (reports, paginationMetadata) = await reportRepository.GetAllReportsAsync(request.Parameters, cancellationToken);

        List<ReportDto> reportDtos = [];

        foreach(var report in reports)
        {
            var reportDto = new ReportDto
            {
                Id = report.Id,
                Content = report.Content,
                Status = report.Status,
                EventId = report.EventId,
                CreationDate = report.CreationDate,
                LastModified = report.LastModified,
                AttendeeId = report.AttendeeId,
                AttendeeName = await userRepository.GetFullNameByUserId(report.Attendee.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Attendee has no Name"),
                AttendeeUserName = await userRepository.GetUserNameByUserId(report.Attendee.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Attendee has no UserName"),
                AttendeeImageUrl = await userRepository.GetProfilePictureByUserId(report.Attendee.UserId, cancellationToken),
                EventName = report.Event.Name,
                OrganizerId = report.Event.OrganizerId,
                OrganizerUserName = await userRepository.GetUserNameByUserId(report.Event.Organizer.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Organizer has no UserName")
            };

            reportDtos.Add(reportDto);
        }

        return (reportDtos, paginationMetadata);
    }
}
