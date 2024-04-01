using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface ICategoryRepository
{
    Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<Category?> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken);
    Task<Category?> GetCategoryByNameAsync(string categoryName, CancellationToken cancellationToken);
    Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<Category> DeleteCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken);
}
