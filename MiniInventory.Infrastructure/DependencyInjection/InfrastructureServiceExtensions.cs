using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Application.Services;
using MiniInventory.Infrastructure.Data;
using MiniInventory.Infrastructure.Repositories;

namespace MiniInventory.Infrastructure.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Registers all Infrastructure and Application layer services,
    /// repositories, and the EF Core DbContext.
    /// Call this from Program.cs: builder.Services.AddInfrastructure(builder.Configuration)
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ─── Database Context ──────────────────────────────────────────────────
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        // ─── Repositories ──────────────────────────────────────────────────────
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IStockInRepository, StockInRepository>();
        services.AddScoped<IStockOutRepository, StockOutRepository>();

        // ─── Services ─────────────────────────────────────────────────────────
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IStockService, StockService>();

        return services;
    }
}
