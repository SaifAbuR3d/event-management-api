using EventManagement.Application.Abstractions.Persistence;
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

    public async Task<Attendee?> GetAttendeeByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Attendees
            .Include(a => a.Followings)
            .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
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

    public Task<Attendee?> GetAttendeeByEmailAsync(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Attendee?> GetAttendeeByUserNameAsync(string userName,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Attendee> UpdateAttendeeAsync(Attendee attendee, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
