using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using MediatR;

namespace EventManagement.Application.Features.Reports.GetReports;

public record GetAllReviewReportsQuery(GetAllReviewReportsQueryParameters Parameters)
    : IRequest<(IEnumerable<ReviewReportDto> Reports, PaginationMetadata PaginationMetadata)>;

public class GetAllReviewReportsQueryHandler(ICurrentUser currentUser,
    IReportRepository reportRepository, IUserRepository userRepository)
    : IRequestHandler<GetAllReviewReportsQuery,
        (IEnumerable<ReviewReportDto> Reports, PaginationMetadata PaginationMetadata)>
{

    public async Task<(IEnumerable<ReviewReportDto> Reports, PaginationMetadata PaginationMetadata)> Handle(GetAllReviewReportsQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAdmin)
        {
            throw new UnauthorizedException("Only admins can view reports");
        }

        var (reports, paginationMetadata) = await reportRepository.GetAllReviewReportsAsync(
            request.Parameters, cancellationToken);

        List<ReviewReportDto> reportDtos = [];

        foreach (var report in reports)
        {
            var reportDto = new ReviewReportDto
            {
                Id = report.Id,
                ReportContent = report.Content,
                Status = report.Status,
                CreationDate = report.CreationDate,
                LastModified = report.LastModified,

                EventId = report.Review.EventId,
                ReviewId = report.Review.Id,
                ReviewWriterId = report.Review.AttendeeId,
                ReviewWriterUserName = await userRepository.GetUserNameByUserId(report.Review.Attendee.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Review Writer has no UserName"),
                ReviewContent = report.Review.Comment,

                ReportWriterId = report.AttendeeId,
                ReportWriterName = await userRepository.GetFullNameByUserId(report.Attendee.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Attendee has no Name"),
                ReportWriterUserName = await userRepository.GetUserNameByUserId(report.Attendee.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Attendee has no UserName"),
                ReportWriterImageUrl = await userRepository.GetProfilePictureByUserId(report.Attendee.UserId, cancellationToken),
            };

            reportDtos.Add(reportDto);
        }

        return (reportDtos, paginationMetadata);
    }
}
