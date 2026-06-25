using MiniInventory.Application.DTOs.Category;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<ApiResponse<List<CategoryDto>>> GetAllAsync();
    Task<ApiResponse<CategoryDto>> GetByIdAsync(int id);
    Task<ApiResponse<List<CategoryDto>>> SearchAsync(string keyword);
    Task<ApiResponse<CategoryDto>> CreateAsync(CategoryCreateDto dto);
    Task<ApiResponse<CategoryDto>> UpdateAsync(int id, CategoryUpdateDto dto);
    Task<ApiResponse> DeleteAsync(int id);
}
