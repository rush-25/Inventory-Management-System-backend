using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Data;

namespace MiniInventory.Infrastructure.Repositories;

public class StockInRepository : IStockInRepository
{
    private readonly AppDbContext _context;

    public StockInRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StockIn>> GetAllAsync()
        => await _context.StockIns
            .Include(s => s.Item)
            .Include(s => s.Supplier)
            .OrderByDescending(s => s.StockInDate)
            .ToListAsync();

    public async Task<int> CreateAsync(StockIn stockIn)
    {
        _context.StockIns.Add(stockIn);
        await _context.SaveChangesAsync();
        return stockIn.StockInId;
    }

    public async Task<int> GetTotalStockInForItemAsync(int itemId)
        => await _context.StockIns
            .Where(s => s.ItemId == itemId)
            .SumAsync(s => s.Quantity);
}

public class StockOutRepository : IStockOutRepository
{
    private readonly AppDbContext _context;

    public StockOutRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StockOut>> GetAllAsync()
        => await _context.StockOuts
            .Include(s => s.Item)
            .OrderByDescending(s => s.StockOutDate)
            .ToListAsync();

    public async Task<int> CreateAsync(StockOut stockOut)
    {
        _context.StockOuts.Add(stockOut);
        await _context.SaveChangesAsync();
        return stockOut.StockOutId;
    }

    public async Task<int> GetTotalStockOutForItemAsync(int itemId)
        => await _context.StockOuts
            .Where(s => s.ItemId == itemId)
            .SumAsync(s => s.Quantity);
}
