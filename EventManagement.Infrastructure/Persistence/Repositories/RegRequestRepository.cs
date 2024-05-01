using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class RegRequestRepository(ApplicationDbContext context) : IRegRequestRepository
{
    public async Task<RegistrationRequest> AddAsync(RegistrationRequest registrationRequest, CancellationToken cancellationToken)
    {
        var entry = await context.RegistrationRequests
            .AddAsync(registrationRequest, cancellationToken);

        return entry.Entity;
    }

    public async Task<RegistrationRequest?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.RegistrationRequests
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}
