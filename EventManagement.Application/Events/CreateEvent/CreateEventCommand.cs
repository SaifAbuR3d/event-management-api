﻿using EventManagement.Application.Abstractions;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Identity;
using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventManagement.Application.Events.CreateEvent;

// support only one category for now

public record CreateEventCommand(string Name, string Description, int CategoryId,
    DateTime StartDate, DateTime EndDate, TimeOnly StartTime, TimeOnly EndTime, 
    double? Lat, double? Lon, string? Street, int? CityId, bool IsOnline, 
    IFormFile Thumbnail, List<IFormFile>? Images, string BaseUrl
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
            if (request.Lat is null || request.Lon is null || request.Street is null || request.CityId is null)
                throw new BadRequestException("Location information is required for offline events.");

            newEvent.SetLocation((double)request.Lat, (double)request.Lon, (string)request.Street, (int)request.CityId);
        }

        newEvent.AddCategory(category);

        var addedEvent = await eventRepository.AddEventAsync(newEvent, cancellationToken);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await SetImages(request, addedEvent);
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