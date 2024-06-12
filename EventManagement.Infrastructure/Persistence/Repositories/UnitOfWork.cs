using EventManagement.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using EventManagement.Domain.Entities;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (context.Database.CurrentTransaction == null)
        {
            throw new InvalidOperationException("Transaction has not been started");
        }

        await context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (context.Database.CurrentTransaction == null)
        {
            throw new InvalidOperationException("Transaction has not been started");
        }
        await context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && e.Entity is Review))
        {
            entry.State = EntityState.Modified;
            entry.CurrentValues["IsDeleted"] = true;
        }

        return await context.SaveChangesAsync(cancellationToken);
    }
}
