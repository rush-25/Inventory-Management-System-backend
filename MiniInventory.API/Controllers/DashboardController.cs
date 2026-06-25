using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.Interfaces.Services;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IStockService _stockService;

    public DashboardController(IStockService stockService)
    {
        _stockService = stockService;
    }

    /// <summary>
    /// Get dashboard summary statistics:
    /// TotalItems, TotalCategories, TotalSuppliers, LowStockItems, OutOfStockItems.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var result = await _stockService.GetDashboardStatsAsync();
        return Ok(result);
    }
}
