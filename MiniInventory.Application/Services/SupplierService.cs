using MiniInventory.Application.DTOs.Supplier;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Domain.Entities;
using MiniInventory.Shared.CommonResponse;
using Microsoft.EntityFrameworkCore;

namespace MiniInventory.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<ApiResponse<List<SupplierDto>>> GetAllAsync()
    {
        var suppliers = await _supplierRepository.GetAllAsync();
        return ApiResponse<List<SupplierDto>>.SuccessResponse(suppliers.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<SupplierDto>> GetByIdAsync(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier is null)
            return ApiResponse<SupplierDto>.FailureResponse($"Supplier with ID {id} not found.");

        return ApiResponse<SupplierDto>.SuccessResponse(MapToDto(supplier));
    }

    public async Task<ApiResponse<SupplierDto>> CreateAsync(SupplierCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.SupplierName))
            return ApiResponse<SupplierDto>.FailureResponse("Supplier name is required.");

        var existingName = await _supplierRepository.GetByNameAsync(dto.SupplierName.Trim());
        if (existingName is not null)
            return ApiResponse<SupplierDto>.FailureResponse($"Supplier with name '{dto.SupplierName}' already exists.");

        var supplier = new Supplier
        {
            SupplierName = dto.SupplierName.Trim(),
            ContactNumber = dto.ContactNumber?.Trim(),
            Email = dto.Email?.Trim(),
            Address = dto.Address?.Trim(),
            IsActive = dto.IsActive,
            CreatedDate = DateTime.UtcNow
        };

        var id = await _supplierRepository.CreateAsync(supplier);
        supplier.SupplierId = id;
        return ApiResponse<SupplierDto>.SuccessResponse(MapToDto(supplier), "Supplier created successfully.");
    }

    public async Task<ApiResponse<SupplierDto>> UpdateAsync(int id, SupplierUpdateDto dto)
    {
        var existing = await _supplierRepository.GetByIdAsync(id);
        if (existing is null)
            return ApiResponse<SupplierDto>.FailureResponse($"Supplier with ID {id} not found.");

        if (string.IsNullOrWhiteSpace(dto.SupplierName))
            return ApiResponse<SupplierDto>.FailureResponse("Supplier name is required.");

        var existingName = await _supplierRepository.GetByNameAsync(dto.SupplierName.Trim());
        if (existingName is not null && existingName.SupplierId != id)
            return ApiResponse<SupplierDto>.FailureResponse($"Supplier with name '{dto.SupplierName}' already exists.");

        existing.SupplierName = dto.SupplierName.Trim();
        existing.ContactNumber = dto.ContactNumber?.Trim();
        existing.Email = dto.Email?.Trim();
        existing.Address = dto.Address?.Trim();
        existing.IsActive = dto.IsActive;

        await _supplierRepository.UpdateAsync(existing);
        return ApiResponse<SupplierDto>.SuccessResponse(MapToDto(existing), "Supplier updated successfully.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var existing = await _supplierRepository.GetByIdAsync(id);
        if (existing is null)
            return ApiResponse.FailureResponse($"Supplier with ID {id} not found.");

        try
        {
            await _supplierRepository.DeleteAsync(id);
            return ApiResponse.SuccessResponse("Supplier deleted successfully.");
        }
        catch (DbUpdateException)
        {
            return ApiResponse.FailureResponse("Cannot delete supplier because it is currently assigned to one or more items.");
        }
        catch (Exception ex)
        {
            return ApiResponse.FailureResponse("An error occurred while deleting the supplier. Please try again.");
        }
    }

    private static SupplierDto MapToDto(Supplier s) => new()
    {
        SupplierId = s.SupplierId,
        SupplierName = s.SupplierName,
        ContactNumber = s.ContactNumber,
        Email = s.Email,
        Address = s.Address,
        IsActive = s.IsActive,
        CreatedDate = s.CreatedDate
    };
}
