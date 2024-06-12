using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Reports.AddReport;

public record AddReviewReportCommand(string Content, int ReviewId) : IRequest<int>;

public class AddReviewReportCommandHandler(ICurrentUser currentUser,
    IReportRepository reportRepository, IReviewRepository reviewRepository,
    IAttendeeRepository attendeeRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddReviewReportCommand, int>
{

    public async Task<int> Handle(AddReviewReportCommand request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAttendee)
        {
            throw new UnauthorizedException("Only attendees can add reports");
        }

        var review = await reviewRepository.GetReviewByIdAsync(request.ReviewId, cancellationToken)
            ?? throw new NotFoundException(nameof(Review), request.ReviewId);

        var attendee = await attendeeRepository.GetAttendeeByUserIdAsync(
            currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Attendee), currentUser.UserId);

        var report = new ReviewReport(request.Content, request.ReviewId, attendee.Id);

        await reportRepository.AddReviewReportAsync(report, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return report.Id;
    }
}