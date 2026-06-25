using MiniInventory.Application.DTOs.Category;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Domain.Entities;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ApiResponse<List<CategoryDto>>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var dtos = categories.Select(MapToDto).ToList();
        return ApiResponse<List<CategoryDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<CategoryDto>> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
            return ApiResponse<CategoryDto>.FailureResponse($"Category with ID {id} not found.");

        return ApiResponse<CategoryDto>.SuccessResponse(MapToDto(category));
    }

    public async Task<ApiResponse<List<CategoryDto>>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await GetAllAsync();

        var categories = await _categoryRepository.SearchAsync(keyword);
        return ApiResponse<List<CategoryDto>>.SuccessResponse(categories.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<CategoryDto>> CreateAsync(CategoryCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CategoryName))
            return ApiResponse<CategoryDto>.FailureResponse("Category name is required.", new List<string> { "CategoryName is required." });

        var category = new Category
        {
            CategoryName = dto.CategoryName.Trim(),
            Description = dto.Description?.Trim(),
            IsActive = dto.IsActive,
            CreatedDate = DateTime.UtcNow
        };

        var id = await _categoryRepository.CreateAsync(category);
        category.CategoryId = id;
        return ApiResponse<CategoryDto>.SuccessResponse(MapToDto(category), "Category created successfully.");
    }

    public async Task<ApiResponse<CategoryDto>> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var existing = await _categoryRepository.GetByIdAsync(id);
        if (existing is null)
            return ApiResponse<CategoryDto>.FailureResponse($"Category with ID {id} not found.");

        if (string.IsNullOrWhiteSpace(dto.CategoryName))
            return ApiResponse<CategoryDto>.FailureResponse("Category name is required.");

        existing.CategoryName = dto.CategoryName.Trim();
        existing.Description = dto.Description?.Trim();
        existing.IsActive = dto.IsActive;

        await _categoryRepository.UpdateAsync(existing);
        return ApiResponse<CategoryDto>.SuccessResponse(MapToDto(existing), "Category updated successfully.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var existing = await _categoryRepository.GetByIdAsync(id);
        if (existing is null)
            return ApiResponse.FailureResponse($"Category with ID {id} not found.");

        await _categoryRepository.DeleteAsync(id);
        return ApiResponse.SuccessResponse("Category deleted successfully.");
    }

    private static CategoryDto MapToDto(Category c) => new()
    {
        CategoryId = c.CategoryId,
        CategoryName = c.CategoryName,
        Description = c.Description,
        IsActive = c.IsActive,
        CreatedDate = c.CreatedDate
    };
}
