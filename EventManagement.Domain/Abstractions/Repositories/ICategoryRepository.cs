using EventManagement.Domain.Models;

namespace EventManagement.Domain.Abstractions.Repositories;

public interface ICategoryRepository
{
    Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<Category?> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken);
    Task<Category?> GetCategoryByNameAsync(string categoryName, CancellationToken cancellationToken);
    Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<Category> DeleteCategoryAsync(Category category, CancellationToken cancellationToken);
}
