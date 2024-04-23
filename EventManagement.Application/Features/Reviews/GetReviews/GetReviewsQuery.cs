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

        List<ReviewDto> reviewDtos = [];

        foreach (var review in reviews)
        {
            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                AttendeeId = review.AttendeeId,
                EventId = review.EventId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreationDate = review.CreationDate,
                LastModified = review.LastModified,
                AttendeeName = await userRepository.GetFullNameByUserId(review.Attendee.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Attendee has no Name"),
                AttendeeUserName = await userRepository.GetUserNameByUserId(review.Attendee.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Attendee has no UserName"),
                AttendeeImageUrl = await userRepository.GetProfilePictureByUserId(review.Attendee.UserId, cancellationToken),
            };

            reviewDtos.Add(reviewDto);
        }

        return (reviewDtos, paginationMetadata);
    }
}