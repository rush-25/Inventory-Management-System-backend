using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Interfaces.Repositories;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllAsync();
    Task<Supplier?> GetByIdAsync(int id);
    Task<int> CreateAsync(Supplier supplier);
    Task<int> UpdateAsync(Supplier supplier);
    Task<int> DeleteAsync(int id);
}
