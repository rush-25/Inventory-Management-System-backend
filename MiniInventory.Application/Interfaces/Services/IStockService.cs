using MiniInventory.Application.DTOs.Dashboard;
using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Interfaces.Services;

public interface IStockService
{
    Task<ApiResponse<StockInDto>> StockInAsync(StockInCreateDto dto);
    Task<ApiResponse<StockOutDto>> StockOutAsync(StockOutCreateDto dto);
    Task<ApiResponse<List<StockBalanceDto>>> GetStockBalanceAsync();
    Task<ApiResponse<List<StockBalanceDto>>> GetLowStockItemsAsync();
    Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync();
}
