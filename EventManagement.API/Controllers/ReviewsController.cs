using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Reviews.AddReview;
using EventManagement.Application.Features.Reviews.DeleteReview;
using EventManagement.Application.Features.Reviews.GetReviews;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventManagement.API.Controllers;

/// <summary>
/// endpoint for reviews
/// </summary>
[ApiController]
[Route("api/events")]
public class ReviewsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Add a review to an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("{eventId}/reviews")]
    public async Task<ActionResult<int>> AddReview(int eventId, CreateReviewRequest request)
    {
        var command = new AddReviewCommand(eventId, request.Rating, request.Title, request.Comment);
        var reviewId = await mediator.Send(command);
        return Ok(new { reviewId });
    }
    /// <summary>
    /// get reviews for an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [HttpGet("{eventId}/reviews")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews(int eventId,
        [FromQuery] GetAllQueryParameters parameters)
    {
        var (reviews, paginationMetadata) = await mediator.Send(
            new GetReviewsQuery(eventId, parameters));
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(reviews);
    }

    /// <summary>
    /// delete a review
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="reviewId"></param>
    /// <returns></returns>
    [HttpDelete("{eventId}/reviews/{reviewId}")]
    public async Task<ActionResult> DeleteReview(int eventId, int reviewId)
    {
        await mediator.Send(new DeleteReviewCommand(eventId, reviewId));
        return NoContent();
    }

}
