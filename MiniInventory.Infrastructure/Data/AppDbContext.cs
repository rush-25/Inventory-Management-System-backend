using Microsoft.EntityFrameworkCore;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<StockIn> StockIns => Set<StockIn>();
    public DbSet<StockOut> StockOuts => Set<StockOut>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ─── Category ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("CategoryTable");
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
        });

        // ─── Supplier ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("SupplierTable");
            entity.HasKey(e => e.SupplierId);
            entity.Property(e => e.SupplierId).ValueGeneratedOnAdd();
            entity.Property(e => e.SupplierName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContactNumber).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
        });

        // ─── Item ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("ItemTable");
            entity.HasKey(e => e.ItemId);
            entity.Property(e => e.ItemId).ValueGeneratedOnAdd();
            entity.Property(e => e.ItemCode).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Barcode).HasMaxLength(100);
            entity.Property(e => e.ItemName).IsRequired().HasMaxLength(300);
            entity.Property(e => e.CostPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.SellingPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            // Indexes for search performance
            entity.HasIndex(e => e.ItemCode).HasDatabaseName("IX_Item_ItemCode");
            entity.HasIndex(e => e.Barcode).HasDatabaseName("IX_Item_Barcode");
            entity.HasIndex(e => e.ItemName).HasDatabaseName("IX_Item_ItemName");
            entity.HasIndex(e => e.CategoryId).HasDatabaseName("IX_Item_CategoryId");
            entity.HasIndex(e => e.SupplierId).HasDatabaseName("IX_Item_SupplierId");

            // FK: Category (restrict delete to avoid accidental category removal)
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Items)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            // FK: Supplier
            entity.HasOne(e => e.Supplier)
                  .WithMany(s => s.Items)
                  .HasForeignKey(e => e.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── StockIn ──────────────────────────────────────────────────────────
        modelBuilder.Entity<StockIn>(entity =>
        {
            entity.ToTable("StockInTable");
            entity.HasKey(e => e.StockInId);
            entity.Property(e => e.StockInId).ValueGeneratedOnAdd();
            entity.Property(e => e.CostPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.StockInDate).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Item)
                  .WithMany(i => i.StockIns)
                  .HasForeignKey(e => e.ItemId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Supplier)
                  .WithMany(s => s.StockIns)
                  .HasForeignKey(e => e.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── StockOut ─────────────────────────────────────────────────────────
        modelBuilder.Entity<StockOut>(entity =>
        {
            entity.ToTable("StockOutTable");
            entity.HasKey(e => e.StockOutId);
            entity.Property(e => e.StockOutId).ValueGeneratedOnAdd();
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(100);
            entity.Property(e => e.StockOutDate).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Item)
                  .WithMany(i => i.StockOuts)
                  .HasForeignKey(e => e.ItemId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
