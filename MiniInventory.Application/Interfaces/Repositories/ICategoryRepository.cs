using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category?> GetByNameAsync(string name);
    Task<List<Category>> SearchAsync(string keyword);
    Task<int> CreateAsync(Category category);
    Task<int> UpdateAsync(Category category);
    Task<int> DeleteAsync(int id);
}
