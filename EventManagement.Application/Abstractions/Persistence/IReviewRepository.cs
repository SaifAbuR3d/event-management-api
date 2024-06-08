using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IReviewRepository
{
    Task<Review> AddReviewAsync(Review review, CancellationToken cancellationToken);
    Task<Review?> GetReviewByIdAsync(int reviewId, CancellationToken cancellationToken);
    Task<Review?> GetReviewByAttendeeIdAndEventIdAsync(int attendeeId, int eventId,
        CancellationToken cancellationToken);
    Task<(IEnumerable<Review>, PaginationMetadata)> GetReviewsByEventIdAsync(int eventId,
        GetAllQueryParameters queryParameters, CancellationToken cancellationToken);
    Task<double?> GetEventAvgRating(int EventId, CancellationToken cancellationToken);
}
