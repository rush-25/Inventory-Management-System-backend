using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Data;

namespace MiniInventory.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
        => await _context.Categories
            .OrderBy(c => c.CategoryName)
            .ToListAsync();

    public async Task<Category?> GetByIdAsync(int id)
        => await _context.Categories.FindAsync(id);

    public async Task<List<Category>> SearchAsync(string keyword)
        => await _context.Categories
            .Where(c => c.CategoryName.Contains(keyword) ||
                        (c.Description != null && c.Description.Contains(keyword)))
            .OrderBy(c => c.CategoryName)
            .ToListAsync();

    public async Task<int> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category.CategoryId;
    }

    public async Task<int> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null) return 0;
        _context.Categories.Remove(category);
        return await _context.SaveChangesAsync();
    }
}
