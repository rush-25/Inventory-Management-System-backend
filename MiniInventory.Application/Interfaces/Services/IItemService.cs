using MiniInventory.Application.DTOs.Item;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Interfaces.Services;

public interface IItemService
{
    Task<ApiResponse<List<ItemDto>>> GetAllAsync();
    Task<ApiResponse<ItemDto>> GetByIdAsync(int id);
    Task<ApiResponse<List<ItemDto>>> SearchAsync(string keyword);
    Task<ApiResponse<ItemDto>> CreateAsync(ItemCreateDto dto);
    Task<ApiResponse<ItemDto>> UpdateAsync(int id, ItemUpdateDto dto);
    Task<ApiResponse> DeleteAsync(int id);
}
