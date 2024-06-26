﻿using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class AttendeeRepository(ApplicationDbContext context)
    : IAttendeeRepository
{
    public async Task<Attendee> AddAttendeeAsync(Attendee attendee, CancellationToken cancellationToken)
    {
        var entry = await context.Attendees.AddAsync(attendee, cancellationToken);
        return entry.Entity;
    }

    public async Task<(IEnumerable<Attendee>, PaginationMetadata)> GetAttendeesFollowingAnOrganizerAsync(int organizerId,
    GetAllQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = context.Followings
            .Where(f => f.OrganizerId == organizerId)
            .Select(f => f.Attendee);

        query = SortingHelper.ApplySorting(query, parameters.SortOrder,
            SortingHelper.AttendeesSortingKeySelector(parameters.SortColumn));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
            parameters.PageIndex, parameters.PageSize, cancellationToken);


        query = PaginationHelper.ApplyPagination(query, parameters.PageIndex, parameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    public async Task<(IEnumerable<Attendee>, PaginationMetadata)> GetAttendeesAsync(
        GetAllAttendeesQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = context.Attendees
            .Include(a => a.Bookings).ThenInclude(b => b.Event)
            .AsQueryable();

        query = SortingHelper.ApplySorting(query, parameters.SortOrder,
                       SortingHelper.AttendeesSortingKeySelector(parameters.SortColumn));

        query = ApplyFilters(parameters, query);

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
                       parameters.PageIndex, parameters.PageSize, cancellationToken);

        query = PaginationHelper.ApplyPagination(query, parameters.PageIndex, parameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    private static IQueryable<Attendee> ApplyFilters(GetAllAttendeesQueryParameters parameters, IQueryable<Attendee> query)
    {
        if (parameters.EventId.HasValue)
        {
            query = query.Where(a => a.Bookings.Any(b => b.EventId == parameters.EventId));
        }
        if (parameters.OnlyVerified.HasValue)
        {
            query = query.Where(a => a.IsVerified == parameters.OnlyVerified);
        }

        return query;
    }

    public async Task<bool> IsFollowingOrganizer(int attendeeId, int organizerId, CancellationToken cancellationToken)
    {
        return await context.Followings
            .AnyAsync(f => f.AttendeeId == attendeeId
                        && f.OrganizerId == organizerId, cancellationToken);
    }

    public async Task UnfollowAnOrganizer(int attendeeId, int organizerId,
        CancellationToken cancellationToken)
    {
        var followingEntity = await context.Followings
            .FirstOrDefaultAsync(f => f.AttendeeId == attendeeId
                                   && f.OrganizerId == organizerId, cancellationToken);

        if (followingEntity != null)
        {
            context.Followings.Remove(followingEntity);
        }
    }

    public async Task<Attendee?> GetAttendeeByUserIdAsync(int userId,
        CancellationToken cancellationToken,
        bool includeFollowings = false, bool includeCategories = false)
    {
        var query = context.Attendees.AsQueryable();
        if(includeFollowings)
        {
            query.Include(a => a.Followings);
        }
        if (includeCategories)
        {
            query.Include(a => a.Categories);
        }

        return await query
            .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
    }

    public async Task<Attendee?> GetAttendeeByUserIdWithCategoriesAsync(int userId,
        CancellationToken cancellationToken)
    {
        return await context.Attendees
            .Include(a => a.Categories)
            .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
    }

    public async Task<Attendee?> GetAttendeeByUserNameAsync(string userName,
    CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        if (user == null)
            return null;

        return await GetAttendeeByUserIdAsync(user.Id, cancellationToken);
    }


    public async Task<bool> HasMadeRegRequest(int attendeeId, int eventId, CancellationToken cancellationToken)
    {
        return await context.RegistrationRequests
            .AnyAsync(r => r.AttendeeId == attendeeId
                        && r.EventId == eventId, cancellationToken);
    }

    public async Task<bool> HasAttendedEvent(int attendeeId, int eventId,
    CancellationToken cancellationToken)
    {
        return await context.Bookings
            .AnyAsync(b => b.AttendeeId == attendeeId
                        && b.EventId == eventId, cancellationToken);
    }

    public async Task<bool> DoesLikeEvent(int attendeeId, int eventId,
        CancellationToken cancellationToken)
    {
        return await context.Likes
            .AnyAsync(l => l.AttendeeId == attendeeId
                        && l.EventId == eventId, cancellationToken);
    }

    public async Task RemoveLikeFromEvent(int attendeeId, int eventId,
        CancellationToken cancellationToken)
    {
        var likeEntity = await context.Likes
            .FirstOrDefaultAsync(l => l.AttendeeId == attendeeId
                                   && l.EventId == eventId, cancellationToken);

        if (likeEntity != null)
        {
            context.Likes.Remove(likeEntity);
        }
    }



    public Task<Attendee> DeleteAttendeeAsync(Attendee attendee, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    public Task<Attendee> UpdateAttendeeAsync(Attendee attendee, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
