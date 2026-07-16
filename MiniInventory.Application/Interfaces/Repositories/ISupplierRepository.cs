using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Interfaces.Repositories;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllAsync();
    Task<Supplier?> GetByIdAsync(int id);
    Task<Supplier?> GetByNameAsync(string name);
    Task<int> CreateAsync(Supplier supplier);
    Task<int> UpdateAsync(Supplier supplier);
    Task<int> DeleteAsync(int id);
}
