using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using MediatR;

namespace EventManagement.Application.Features.Reviews.GetReviews;

public record GetReviewsQuery(int EventId, GetAllQueryParameters Parameters) 
    : IRequest<(IEnumerable<ReviewDto>, PaginationMetadata)>;

public class GetReviewsQueryHandler(IReviewRepository reviewRepository, 
    IUserRepository userRepository)
    : IRequestHandler<GetReviewsQuery, (IEnumerable<ReviewDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<ReviewDto>, PaginationMetadata)> Handle(GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var (reviews, paginationMetadata) = await reviewRepository.GetReviewsByEventIdAsync(request.EventId,
            request.Parameters, cancellationToken);

        var reviewDtos = reviews.Select(async r => new ReviewDto
        {
            Id = r.Id,
            AttendeeId = r.AttendeeId,
            EventId = r.EventId,
            Rating = r.Rating,
            Comment = r.Comment,
            CreationDate = r.CreationDate, 
            LastModified = r.LastModified,
            AttendeeName = await userRepository.GetFullNameByUserId(r.Attendee.UserId, cancellationToken)
                ?? throw new CustomException("Invalid State: Attendee has no Name"),
            AttendeeUserName = await userRepository.GetUserNameByUserId(r.Attendee.UserId, cancellationToken)
                ?? throw new CustomException("Invalid State: Attendee has no UserName"),
            AttendeeImageUrl = await userRepository.GetProfilePictureByUserId(r.Attendee.UserId, cancellationToken)
        });

        return (await Task.WhenAll(reviewDtos), paginationMetadata);
    }
}