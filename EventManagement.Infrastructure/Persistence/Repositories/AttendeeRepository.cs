using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Attendee?> GetAttendeeByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Attendees.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
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
