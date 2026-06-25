using MiniInventory.Application.DTOs.Supplier;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Interfaces.Services;

public interface ISupplierService
{
    Task<ApiResponse<List<SupplierDto>>> GetAllAsync();
    Task<ApiResponse<SupplierDto>> GetByIdAsync(int id);
    Task<ApiResponse<SupplierDto>> CreateAsync(SupplierCreateDto dto);
    Task<ApiResponse<SupplierDto>> UpdateAsync(int id, SupplierUpdateDto dto);
    Task<ApiResponse> DeleteAsync(int id);
}
