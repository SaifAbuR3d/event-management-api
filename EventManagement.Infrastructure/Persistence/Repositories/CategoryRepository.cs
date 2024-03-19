using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Domain.Entities;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        var entry = await context.Categories.AddAsync(category, cancellationToken);
        return entry.Entity;
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken)
    {
        return await context.Categories.FindAsync(categoryId, cancellationToken); 
    }


    public Task<Category> DeleteCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Category?> GetCategoryByNameAsync(string categoryName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
