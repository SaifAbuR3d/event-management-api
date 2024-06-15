using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Attendees.SetAttendeeInterests;
public record SetAttendeeInterestsCommand(List<int> CategoryIds) : IRequest<Unit>; 

public class SetAttendeeInterestsCommandHandler(ICurrentUser currentUser, 
    IAttendeeRepository attendeeRepository,
    ICategoryRepository categoryRepository, 
    IUnitOfWork unitOfWork) : IRequestHandler<SetAttendeeInterestsCommand, Unit>
{

    public async Task<Unit> Handle(SetAttendeeInterestsCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        AuthorizeCurrentUser();

        var attendee = await attendeeRepository.GetAttendeeByUserIdWithCategoriesAsync(currentUser.UserId, cancellationToken) ?? 
            throw new NotFoundException( nameof(Attendee), nameof(currentUser.UserId), currentUser.UserId);

        attendee.ResetInterests();

        foreach (var categoryId in request.CategoryIds)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(categoryId, cancellationToken)
                ?? throw new NotFoundException(nameof(Category), categoryId);

            attendee.AddInterest(category);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private void AuthorizeCurrentUser()
    {
        if (!currentUser.IsAttendee)
        {
            throw new UnauthorizedException("Only attendees can set their interests");
        }
    }

    private static void Validate(SetAttendeeInterestsCommand request)
    {
        if (request.CategoryIds == null || request.CategoryIds.Count == 0)
        {
            throw new BadRequestException("CategoryIds cannot be null or empty");
        }

        if (request.CategoryIds.Count > 4)
        {
            throw new BadRequestException("Maximum 4 categories are allowed");
        }
    }
}