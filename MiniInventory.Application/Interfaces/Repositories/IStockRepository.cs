using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Interfaces.Repositories;

public interface IStockInRepository
{
    Task<List<StockIn>> GetAllAsync();
    Task<int> CreateAsync(StockIn stockIn);
    Task<int> GetTotalStockInForItemAsync(int itemId);
}

public interface IStockOutRepository
{
    Task<List<StockOut>> GetAllAsync();
    Task<int> CreateAsync(StockOut stockOut);
    Task<int> GetTotalStockOutForItemAsync(int itemId);
}
