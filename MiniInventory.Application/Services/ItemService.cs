using MiniInventory.Application.DTOs.Item;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Domain.Entities;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ApiResponse<List<ItemDto>>> GetAllAsync()
    {
        var items = await _itemRepository.GetAllAsync();
        return ApiResponse<List<ItemDto>>.SuccessResponse(items.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<ItemDto>> GetByIdAsync(int id)
    {
        var item = await _itemRepository.GetByIdAsync(id);
        if (item is null)
            return ApiResponse<ItemDto>.FailureResponse($"Item with ID {id} not found.");

        return ApiResponse<ItemDto>.SuccessResponse(MapToDto(item));
    }

    public async Task<ApiResponse<List<ItemDto>>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await GetAllAsync();

        var items = await _itemRepository.SearchAsync(keyword);
        return ApiResponse<List<ItemDto>>.SuccessResponse(items.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<ItemDto>> CreateAsync(ItemCreateDto dto)
    {
        var errors = Validate(dto);
        if (errors.Any())
            return ApiResponse<ItemDto>.FailureResponse("Validation failed.", errors);

        var item = new Item
        {
            ItemCode = dto.ItemCode.Trim(),
            Barcode = dto.Barcode?.Trim(),
            ItemName = dto.ItemName.Trim(),
            Brand = dto.Brand?.Trim(),
            ImageUrl = dto.ImageUrl?.Trim(),
            CategoryId = dto.CategoryId,
            SupplierId = dto.SupplierId,
            CostPrice = dto.CostPrice,
            SellingPrice = dto.SellingPrice,
            ReorderLevel = dto.ReorderLevel,
            IsActive = dto.IsActive,
            CreatedDate = DateTime.UtcNow
        };

        var id = await _itemRepository.CreateAsync(item);
        item.ItemId = id;
        return ApiResponse<ItemDto>.SuccessResponse(MapToDto(item), "Item created successfully.");
    }

    public async Task<ApiResponse<ItemDto>> UpdateAsync(int id, ItemUpdateDto dto)
    {
        var existing = await _itemRepository.GetByIdAsync(id);
        if (existing is null)
            return ApiResponse<ItemDto>.FailureResponse($"Item with ID {id} not found.");

        existing.ItemCode = dto.ItemCode.Trim();
        existing.Barcode = dto.Barcode?.Trim();
        existing.ItemName = dto.ItemName.Trim();
        existing.Brand = dto.Brand?.Trim();
        existing.ImageUrl = dto.ImageUrl?.Trim();
        existing.CategoryId = dto.CategoryId;
        existing.SupplierId = dto.SupplierId;
        existing.CostPrice = dto.CostPrice;
        existing.SellingPrice = dto.SellingPrice;
        existing.ReorderLevel = dto.ReorderLevel;
        existing.IsActive = dto.IsActive;

        await _itemRepository.UpdateAsync(existing);
        return ApiResponse<ItemDto>.SuccessResponse(MapToDto(existing), "Item updated successfully.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var existing = await _itemRepository.GetByIdAsync(id);
        if (existing is null)
            return ApiResponse.FailureResponse($"Item with ID {id} not found.");

        await _itemRepository.DeleteAsync(id);
        return ApiResponse.SuccessResponse("Item deleted successfully.");
    }

    // ─── Private Helpers ───────────────────────────────────────────────────────

    private static List<string> Validate(ItemCreateDto dto)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(dto.ItemCode)) errors.Add("ItemCode is required.");
        if (string.IsNullOrWhiteSpace(dto.ItemName)) errors.Add("ItemName is required.");
        if (dto.CategoryId <= 0) errors.Add("CategoryId must be a valid positive integer.");
        if (dto.SupplierId <= 0) errors.Add("SupplierId must be a valid positive integer.");
        if (dto.CostPrice < 0) errors.Add("CostPrice cannot be negative.");
        if (dto.SellingPrice < 0) errors.Add("SellingPrice cannot be negative.");
        if (dto.ReorderLevel < 0) errors.Add("ReorderLevel cannot be negative.");
        return errors;
    }

    private static ItemDto MapToDto(Item i) => new()
    {
        ItemId = i.ItemId,
        ItemCode = i.ItemCode,
        Barcode = i.Barcode,
        ItemName = i.ItemName,
        Brand = i.Brand,
        ImageUrl = i.ImageUrl,
        CategoryId = i.CategoryId,
        CategoryName = i.Category?.CategoryName,
        SupplierId = i.SupplierId,
        SupplierName = i.Supplier?.SupplierName,
        CostPrice = i.CostPrice,
        SellingPrice = i.SellingPrice,
        ReorderLevel = i.ReorderLevel,
        IsActive = i.IsActive,
        CreatedDate = i.CreatedDate
    };
}
