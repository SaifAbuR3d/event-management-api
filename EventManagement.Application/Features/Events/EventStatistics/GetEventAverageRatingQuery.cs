using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Events.EventStatistics;

public record GetEventAverageRatingQuery(int EventId) : IRequest<double>;

public class GetEventAverageRatingQueryHandler(IEventRepository eventRepository,
    IReviewRepository reviewRepository)
    : IRequestHandler<GetEventAverageRatingQuery, double>
{
    public async Task<double> Handle(GetEventAverageRatingQuery request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        if(!@event.HasEnded())
        {
            //throw new BadRequestException("The Event Is Still Running, No Ratings"); 
        }

        double? avg = await reviewRepository.GetEventAvgRating(@event.Id, cancellationToken);

        // if avg is null, there is no ratings for this event yet, return -1 to indicate that

        return avg is null ? -1 : (double)avg; 
    }

}