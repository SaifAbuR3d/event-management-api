using EventManagement.Application.Abstractions.Images;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventManagement.Application.Features.Events.CreateEvent;

// support only one category for now

public record CreateEventCommand(string Name, string Description, int CategoryId,
    DateTime StartDate, DateTime EndDate, TimeOnly StartTime, TimeOnly EndTime,
    double? Lat, double? Lon, string? Street, int? CityId, bool IsOnline,
    IFormFile Thumbnail, List<IFormFile>? Images, List<CreateTicketRequest> Tickets,
    bool IsManaged, int? MinAge, int? MaxAge, Gender? AllowedGender,
    string BaseUrl
    )
    : IRequest<int>;

public class CreateEventCommandHandler(IValidator<CreateEventCommand> validator,
    IEventRepository eventRepository,
    ICategoryRepository categoryRepository,
    IOrganizerRepository organizerRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IImageHandler imageHandler)
    : IRequestHandler<CreateEventCommand, int>
{
    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        if (!currentUser.IsOrganizer)
            throw new UnauthorizedException("Only organizers can create events.");

        var organizer = await organizerRepository.GetOrganizerByUserIdAsync(currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Organizer), nameof(currentUser.UserId), currentUser.UserId);

        var category = await categoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new NotFoundException(nameof(Category), request.CategoryId);

        var startDate = DateOnly.FromDateTime(request.StartDate);
        var endDate = DateOnly.FromDateTime(request.EndDate);

        var newEvent = new Event(organizer, request.Name, request.Description,
            startDate, endDate, request.StartTime, request.EndTime,
            request.IsOnline);

        if (!request.IsOnline)
        {
            if (request.Lat is null || request.Lon is null)
                throw new BadRequestException("Location information(Lat, Long) is required for offline events.");

            newEvent.SetLocation((double)request.Lat, (double)request.Lon, request.Street, request.CityId);
        }

        newEvent.AddCategory(category);

        var addedEvent = await eventRepository.AddEventAsync(newEvent, cancellationToken);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            SetLimitations(request, organizer, addedEvent);

            await unitOfWork.SaveChangesAsync(cancellationToken); // setting id for the event in database

            await SetImages(request, addedEvent); // we need the id of the addedEvent to persist the images

            // TODO: do logic validation for tickets, e.g. tickets quantity should be within user plan limits...
            SetTickets(request, addedEvent);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }


        return addedEvent.Id;
    }

    private void SetLimitations(CreateEventCommand request, Organizer organizer, Event addedEvent)
    {
        if (!organizer.IsVerified)
            throw new BadRequestException("Organizer is not verified.");

        if (request.IsManaged)
        {
            addedEvent.SetLimitations(request.MinAge, request.MaxAge, request.AllowedGender);
        }
    }

    private void SetTickets(CreateEventCommand request, Event addedEvent)
    {
        foreach (var ticket in request.Tickets)
        {
            addedEvent.AddTicket(ticket.Name, ticket.Price, ticket.TotalQuantity, ticket.StartSale, ticket.EndSale);
        }
    }

    private async Task SetImages(CreateEventCommand request, Event @event)
    {

        var eventDirectory = Path.Combine(request.BaseUrl, "images", "events", @event.Id.ToString());

        var thumbnailUrl = await imageHandler.UploadImage(request.Thumbnail, eventDirectory, true);
        @event.SetThumbnail(thumbnailUrl);

        foreach (var image in request.Images ?? Enumerable.Empty<IFormFile>())
        {
            var newImageUrl = await imageHandler.UploadImage(image, eventDirectory, false);
            @event.AddImage(newImageUrl);
        }
    }
}