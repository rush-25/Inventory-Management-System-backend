namespace MiniInventory.Domain.Entities;

public class StockIn
{
    public int StockInId { get; set; }
    public int ItemId { get; set; }
    public int SupplierId { get; set; }
    public int Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public DateTime StockInDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation
    public Item Item { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;
}
