using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class ReviewRepository(ApplicationDbContext context) : IReviewRepository
{
    public async Task<Review> AddReviewAsync(Review review, CancellationToken cancellationToken)
    {
        var entry = await context.Reviews.AddAsync(review, cancellationToken);
        return entry.Entity;
    }

    public async Task<Review?> GetReviewByAttendeeIdAndEventIdAsync(int attendeeId, int eventId, CancellationToken cancellationToken)
    {
        return await context.Reviews
            .FirstOrDefaultAsync(r => r.AttendeeId == attendeeId
                                   && r.EventId == eventId, cancellationToken);
    }

    public async Task<Review?> GetReviewByIdAsync(int reviewId, CancellationToken cancellationToken)
    {
        return await context.Reviews.FindAsync(reviewId, cancellationToken);
    }

    public async Task<(IEnumerable<Review>, PaginationMetadata)> GetReviewsByEventIdAsync(int eventId, GetAllQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = context.Reviews
            .Include(r => r.Attendee)
            .Where(r => r.EventId == eventId);

        query = SortingHelper.ApplySorting(query, parameters.SortOrder,
            SortingHelper.ReviewsSortingKeySelector(parameters.SortColumn));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
            parameters.PageIndex, parameters.PageSize, cancellationToken);

        query = PaginationHelper.ApplyPagination(query, parameters.PageIndex, parameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    public async Task<double?> GetEventAvgRating(int EventId, CancellationToken cancellationToken)
    {
        bool isThereAny = await context.Reviews.AnyAsync(r => r.EventId == EventId, cancellationToken);
        if (!isThereAny)
        {
            return null; 
        }

        return await context.Reviews
            .Where(r => r.EventId == EventId)
            .AverageAsync(r => r.Rating, cancellationToken);
    }
}
