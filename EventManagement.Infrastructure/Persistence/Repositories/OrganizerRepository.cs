using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
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
        return await context.Organizers.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Organizer?> GetOrganizerByUserIdAsync(int userId,
    CancellationToken cancellationToken)
    {
        return await context.Organizers.FirstOrDefaultAsync(o => o.UserId == userId, cancellationToken);
    }

    public async Task<Organizer?> GetOrganizerByUserNameAsync(string userName,
        CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        if (user == null)
            return null;

        return await context.Organizers.FirstOrDefaultAsync(o => o.UserId == user.Id, cancellationToken);
    }
    public async Task<Organizer?> GetOrganizerByEmailAsync(string email,
    CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user == null)
            return null;

        return await context.Organizers.FirstOrDefaultAsync(o => o.UserId == user.Id, cancellationToken);
    }

    public Task<Organizer> DeleteOrganizerAsync(Organizer organizer,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<Attendee>, PaginationMetadata)> GetFollowersByOrganizerIdAsync(int organizerId)
    {
        throw new NotImplementedException();
    }
}
