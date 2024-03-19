using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IOrganizerRepository
{
    Task<Organizer> AddOrganizerAsync(Organizer organizer, CancellationToken cancellationToken);
    Task<Organizer?> GetOrganizerByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Organizer?> GetOrganizerByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<Organizer?> GetOrganizerByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Organizer> DeleteOrganizerAsync(Organizer organizer, CancellationToken cancellationToken);
}
