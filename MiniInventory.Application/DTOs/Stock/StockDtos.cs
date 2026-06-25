namespace MiniInventory.Application.DTOs.Stock;

// ─── Stock In ────────────────────────────────────────────────────────────────

public class StockInDto
{
    public int StockInId { get; set; }
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public int Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public DateTime StockInDate { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class StockInCreateDto
{
    public int ItemId { get; set; }
    public int SupplierId { get; set; }
    public int Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public DateTime StockInDate { get; set; } = DateTime.UtcNow;
}

// ─── Stock Out ────────────────────────────────────────────────────────────────

public class StockOutDto
{
    public int StockOutId { get; set; }
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime StockOutDate { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class StockOutCreateDto
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }

    /// <summary>
    /// Allowed values: Sale, Damage, Internal Use, Return
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    public DateTime StockOutDate { get; set; } = DateTime.UtcNow;
}

// ─── Stock Balance Report ─────────────────────────────────────────────────────

public class StockBalanceDto
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public int TotalStockIn { get; set; }
    public int TotalStockOut { get; set; }
    public int CurrentBalance { get; set; }
    public int ReorderLevel { get; set; }
    public string StockStatus { get; set; } = string.Empty; // Good Stock | Low Stock | Out of Stock
}
