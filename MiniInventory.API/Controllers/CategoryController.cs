using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.DTOs.Category;
using MiniInventory.Application.Interfaces.Services;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>Get all categories.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryService.GetAllAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Get a category by ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>Search categories by name or description.</summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var result = await _categoryService.SearchAsync(keyword);
        return Ok(result);
    }

    /// <summary>Create a new category.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
    {
        var result = await _categoryService.CreateAsync(dto);
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.CategoryId }, result)
            : BadRequest(result);
    }

    /// <summary>Update an existing category.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
    {
        var result = await _categoryService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Delete a category by ID.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
