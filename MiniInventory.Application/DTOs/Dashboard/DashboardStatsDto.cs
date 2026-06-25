namespace MiniInventory.Application.DTOs.Dashboard;

public class DashboardStatsDto
{
    public int TotalItems { get; set; }
    public int TotalCategories { get; set; }
    public int TotalSuppliers { get; set; }
    public int LowStockItems { get; set; }
    public int OutOfStockItems { get; set; }
    public int TotalStockInTransactions { get; set; }
    public int TotalStockOutTransactions { get; set; }
}
