namespace MiniInventory.Domain.Entities;

public class Item
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation
    public Category Category { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;
    public ICollection<StockIn> StockIns { get; set; } = new List<StockIn>();
    public ICollection<StockOut> StockOuts { get; set; } = new List<StockOut>();
}
