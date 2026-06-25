using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Interfaces.Repositories;

public interface IItemRepository
{
    Task<List<Item>> GetAllAsync();
    Task<Item?> GetByIdAsync(int id);
    Task<List<Item>> SearchAsync(string keyword);
    Task<int> CreateAsync(Item item);
    Task<int> UpdateAsync(Item item);
    Task<int> DeleteAsync(int id);
}
