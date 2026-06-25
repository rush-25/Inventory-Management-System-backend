using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.DTOs.Supplier;
using MiniInventory.Application.Interfaces.Services;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    /// <summary>Get all suppliers.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _supplierService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>Get a supplier by ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _supplierService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>Create a new supplier.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SupplierCreateDto dto)
    {
        var result = await _supplierService.CreateAsync(dto);
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.SupplierId }, result)
            : BadRequest(result);
    }

    /// <summary>Update an existing supplier.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SupplierUpdateDto dto)
    {
        var result = await _supplierService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Delete a supplier by ID.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _supplierService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
