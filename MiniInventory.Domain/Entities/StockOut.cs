namespace MiniInventory.Domain.Entities;

public class StockOut
{
    public int StockOutId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }

    /// <summary>
    /// Reason for stock removal: Sale, Damage, Internal Use, Return
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    public DateTime StockOutDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation
    public Item Item { get; set; } = null!;
}
