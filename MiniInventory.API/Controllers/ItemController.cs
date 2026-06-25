using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.DTOs.Item;
using MiniInventory.Application.Interfaces.Services;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    /// <summary>Get all items with category and supplier names.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _itemService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>Get an item by ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _itemService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>Search items by name, item code, or barcode.</summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var result = await _itemService.SearchAsync(keyword);
        return Ok(result);
    }

    /// <summary>Create a new item.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ItemCreateDto dto)
    {
        var result = await _itemService.CreateAsync(dto);
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.ItemId }, result)
            : BadRequest(result);
    }

    /// <summary>Update an existing item.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ItemUpdateDto dto)
    {
        var result = await _itemService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Delete an item by ID.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _itemService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
