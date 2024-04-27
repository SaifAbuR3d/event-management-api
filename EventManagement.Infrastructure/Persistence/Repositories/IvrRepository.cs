using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

internal class IvrRepository(ApplicationDbContext context) : IIvrRepository
{
    public async Task<IdentityVerificationRequest> AddAsync(IdentityVerificationRequest ivr, CancellationToken cancellationToken)
    {
        var entry = await context.IdentityVerificationRequests.AddAsync(ivr, cancellationToken);
        return entry.Entity;
    }

    public async Task<bool> HasPendingRequests(int userId, CancellationToken cancellationToken)
    {
        return await context.IdentityVerificationRequests
                    .AnyAsync(ivr => ivr.UserId == userId
                            && ivr.Status == IdentityVerificationRequestStatus.Pending,
                            cancellationToken);

    }

    public Task<(IEnumerable<IdentityVerificationRequest>, PaginationMetadata)> GetAllAsync(IdentityVerificationRequestStatus status, GetAllIvrsQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityVerificationRequest?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityVerificationRequest?> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
