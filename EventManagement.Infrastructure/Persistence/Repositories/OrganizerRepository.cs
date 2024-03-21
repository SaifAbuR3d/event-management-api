using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class OrganizerRepository(ApplicationDbContext context)
    : IOrganizerRepository
{
    public async Task<Organizer> AddOrganizerAsync(Organizer organizer,
        CancellationToken cancellationToken)
    {
        var entry = await context.Organizers.AddAsync(organizer, cancellationToken);
        return entry.Entity;
    }

    public async Task<Organizer?> GetOrganizerByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Organizers
            .Include(o => o.Profile)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Organizer?> GetOrganizerByUserIdAsync(int userId,
    CancellationToken cancellationToken)
    {
        return await context.Organizers
            .Include(o => o.Profile)
            .FirstOrDefaultAsync(o => o.UserId == userId, cancellationToken);
    }

    public async Task<Organizer?> GetOrganizerByUserNameAsync(string userName,
        CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        if (user == null)
            return null;

        return await GetOrganizerByUserIdAsync(user.Id, cancellationToken); 
    }
    public async Task<Organizer?> GetOrganizerByEmailAsync(string email,
    CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user == null)
            return null;

        return await GetOrganizerByUserIdAsync(user.Id, cancellationToken);
    }

    public async Task<(IEnumerable<Organizer>, PaginationMetadata)> GetOrganizersFollowedByAttendee(int attendeeId,
    GetAllQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = context.Followings
            .Where(f => f.AttendeeId == attendeeId)
            .Select(f => f.Organizer);

        query = SortingHelper.ApplySorting(query,
            parameters.SortOrder, SortingHelper.OrganizersSortingKeySelector(parameters.SortColumn));

        var paginationMetadata = await PaginationHelper.GetPaginationMetadataAsync(query,
            parameters.PageIndex, parameters.PageSize, cancellationToken);

        query = PaginationHelper.ApplyPagination(query, parameters.PageIndex, parameters.PageSize);

        var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (result, paginationMetadata);
    }

    public Task<Organizer> DeleteOrganizerAsync(Organizer organizer,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
