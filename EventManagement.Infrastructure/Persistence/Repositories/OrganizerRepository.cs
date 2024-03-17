using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Models;
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

    public async Task<Organizer?> GetOrganizerByUserIdAsync(int userId,
    CancellationToken cancellationToken)
    {
        return await context.Organizers.FirstOrDefaultAsync(o => o.UserId == userId, cancellationToken);
    }

    public Task<Organizer> DeleteOrganizerAsync(Organizer organizer,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Organizer?> GetOrganizerByEmailAsync(string email,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    public Task<Organizer?> GetOrganizerByUserNameAsync(string userName,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
