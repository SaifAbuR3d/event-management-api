﻿using EventManagement.Application.Exceptions;
using EventManagement.Application.Identity;
using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Models;
using MediatR;

namespace EventManagement.Application.Events.CreateEvent;

// support only one category for now

public record CreateEventCommand(string Name, string Description, int CategoryId,
    DateTime StartDate, DateTime EndDate, TimeOnly StartTime, TimeOnly EndTime, 
    double? Lat, double? Lon, string? Street, int? CityId, bool IsOnline
    )
    : IRequest<int>;

public class CreateEventCommandHandler(IEventRepository eventRepository, 
    ICategoryRepository categoryRepository, 
    IOrganizerRepository organizerRepository,
    IUnitOfWork unitOfWork, 
    ICurrentUser currentUser)
    : IRequestHandler<CreateEventCommand, int>
{
    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        if(currentUser.IsOrganizer)
            throw new UnauthorizedException("Only organizers can create events.");

        var organizer = await organizerRepository.GetOrganizerByUserIdAsync(currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Organizer), nameof(currentUser.UserId), currentUser.UserId);

        var category = await categoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new NotFoundException(nameof(Category), request.CategoryId);

        var startDate = DateOnly.FromDateTime(request.StartDate);
        var endDate = DateOnly.FromDateTime(request.EndDate);

        // check date and time

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
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return addedEvent.Id;
    }
}