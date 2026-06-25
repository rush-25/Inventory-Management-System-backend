using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Data;

namespace MiniInventory.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> GetAllAsync()
        => await _context.Items
            .Include(i => i.Category)
            .Include(i => i.Supplier)
            .OrderBy(i => i.ItemName)
            .ToListAsync();

    public async Task<Item?> GetByIdAsync(int id)
        => await _context.Items
            .Include(i => i.Category)
            .Include(i => i.Supplier)
            .FirstOrDefaultAsync(i => i.ItemId == id);

    public async Task<List<Item>> SearchAsync(string keyword)
        => await _context.Items
            .Include(i => i.Category)
            .Include(i => i.Supplier)
            .Where(i => i.ItemName.Contains(keyword) ||
                        i.ItemCode.Contains(keyword) ||
                        (i.Barcode != null && i.Barcode.Contains(keyword)))
            .OrderBy(i => i.ItemName)
            .ToListAsync();

    public async Task<int> CreateAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        return item.ItemId;
    }

    public async Task<int> UpdateAsync(Item item)
    {
        _context.Items.Update(item);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item is null) return 0;
        _context.Items.Remove(item);
        return await _context.SaveChangesAsync();
    }
}
