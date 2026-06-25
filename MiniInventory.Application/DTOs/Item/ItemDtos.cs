namespace MiniInventory.Application.DTOs.Item;

public class ItemDto
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ItemCreateDto
{
    public string ItemCode { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ItemUpdateDto
{
    public string ItemCode { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; }
}
