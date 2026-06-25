namespace MiniInventory.Domain.Entities;

public class Supplier
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Item> Items { get; set; } = new List<Item>();
    public ICollection<StockIn> StockIns { get; set; } = new List<StockIn>();
}
