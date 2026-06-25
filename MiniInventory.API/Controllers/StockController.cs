using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Application.Interfaces.Services;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    /// <summary>Record a Stock In transaction.</summary>
    [HttpPost("in")]
    public async Task<IActionResult> StockIn([FromBody] StockInCreateDto dto)
    {
        var result = await _stockService.StockInAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Record a Stock Out transaction. Reason: Sale | Damage | Internal Use | Return</summary>
    [HttpPost("out")]
    public async Task<IActionResult> StockOut([FromBody] StockOutCreateDto dto)
    {
        var result = await _stockService.StockOutAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Get the full stock balance report for all items.</summary>
    [HttpGet("balance")]
    public async Task<IActionResult> GetStockBalance()
    {
        var result = await _stockService.GetStockBalanceAsync();
        return Ok(result);
    }

    /// <summary>Get only items with Low Stock or Out of Stock status.</summary>
    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        var result = await _stockService.GetLowStockItemsAsync();
        return Ok(result);
    }
}
