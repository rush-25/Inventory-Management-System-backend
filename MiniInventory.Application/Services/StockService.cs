using MiniInventory.Application.DTOs.Dashboard;
using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Domain.Entities;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Services;

public class StockService : IStockService
{
    private readonly IStockInRepository _stockInRepository;
    private readonly IStockOutRepository _stockOutRepository;
    private readonly IItemRepository _itemRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISupplierRepository _supplierRepository;

    public StockService(
        IStockInRepository stockInRepository,
        IStockOutRepository stockOutRepository,
        IItemRepository itemRepository,
        ICategoryRepository categoryRepository,
        ISupplierRepository supplierRepository)
    {
        _stockInRepository = stockInRepository;
        _stockOutRepository = stockOutRepository;
        _itemRepository = itemRepository;
        _categoryRepository = categoryRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task<ApiResponse<StockInDto>> StockInAsync(StockInCreateDto dto)
    {
        // Validate item exists
        var item = await _itemRepository.GetByIdAsync(dto.ItemId);
        if (item is null)
            return ApiResponse<StockInDto>.FailureResponse($"Item with ID {dto.ItemId} not found.");

        if (dto.Quantity <= 0)
            return ApiResponse<StockInDto>.FailureResponse("Quantity must be greater than zero.");

        var stockIn = new StockIn
        {
            ItemId = dto.ItemId,
            SupplierId = dto.SupplierId,
            Quantity = dto.Quantity,
            CostPrice = dto.CostPrice,
            StockInDate = dto.StockInDate == default ? DateTime.UtcNow : dto.StockInDate,
            CreatedDate = DateTime.UtcNow
        };

        var id = await _stockInRepository.CreateAsync(stockIn);
        stockIn.StockInId = id;

        return ApiResponse<StockInDto>.SuccessResponse(new StockInDto
        {
            StockInId = stockIn.StockInId,
            ItemId = stockIn.ItemId,
            ItemName = item.ItemName,
            SupplierId = stockIn.SupplierId,
            SupplierName = item.Supplier?.SupplierName,
            Quantity = stockIn.Quantity,
            CostPrice = stockIn.CostPrice,
            StockInDate = stockIn.StockInDate,
            CreatedDate = stockIn.CreatedDate
        }, "Stock In recorded successfully.");
    }

    public async Task<ApiResponse<StockOutDto>> StockOutAsync(StockOutCreateDto dto)
    {
        var item = await _itemRepository.GetByIdAsync(dto.ItemId);
        if (item is null)
            return ApiResponse<StockOutDto>.FailureResponse($"Item with ID {dto.ItemId} not found.");

        if (dto.Quantity <= 0)
            return ApiResponse<StockOutDto>.FailureResponse("Quantity must be greater than zero.");

        // Verify we have enough stock
        var totalIn = await _stockInRepository.GetTotalStockInForItemAsync(dto.ItemId);
        var totalOut = await _stockOutRepository.GetTotalStockOutForItemAsync(dto.ItemId);
        var currentBalance = totalIn - totalOut;

        if (dto.Quantity > currentBalance)
            return ApiResponse<StockOutDto>.FailureResponse(
                $"Insufficient stock. Current balance: {currentBalance}, Requested: {dto.Quantity}.");

        var validReasons = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "Sale", "Damage", "Internal Use", "Return" };

        if (!validReasons.Contains(dto.Reason))
            return ApiResponse<StockOutDto>.FailureResponse(
                "Invalid reason. Must be one of: Sale, Damage, Internal Use, Return.");

        var stockOut = new StockOut
        {
            ItemId = dto.ItemId,
            Quantity = dto.Quantity,
            Reason = dto.Reason,
            StockOutDate = dto.StockOutDate == default ? DateTime.UtcNow : dto.StockOutDate,
            CreatedDate = DateTime.UtcNow
        };

        var id = await _stockOutRepository.CreateAsync(stockOut);
        stockOut.StockOutId = id;

        return ApiResponse<StockOutDto>.SuccessResponse(new StockOutDto
        {
            StockOutId = stockOut.StockOutId,
            ItemId = stockOut.ItemId,
            ItemName = item.ItemName,
            Quantity = stockOut.Quantity,
            Reason = stockOut.Reason,
            StockOutDate = stockOut.StockOutDate,
            CreatedDate = stockOut.CreatedDate
        }, "Stock Out recorded successfully.");
    }

    public async Task<ApiResponse<List<StockBalanceDto>>> GetStockBalanceAsync()
    {
        var items = await _itemRepository.GetAllAsync();
        var balances = new List<StockBalanceDto>();

        foreach (var item in items)
        {
            var totalIn = await _stockInRepository.GetTotalStockInForItemAsync(item.ItemId);
            var totalOut = await _stockOutRepository.GetTotalStockOutForItemAsync(item.ItemId);
            var currentBalance = totalIn - totalOut;

            balances.Add(new StockBalanceDto
            {
                ItemId = item.ItemId,
                ItemCode = item.ItemCode,
                ItemName = item.ItemName,
                CategoryName = item.Category?.CategoryName,
                TotalStockIn = totalIn,
                TotalStockOut = totalOut,
                CurrentBalance = currentBalance,
                ReorderLevel = item.ReorderLevel,
                StockStatus = GetStockStatus(currentBalance, item.ReorderLevel)
            });
        }

        return ApiResponse<List<StockBalanceDto>>.SuccessResponse(balances);
    }

    public async Task<ApiResponse<List<StockBalanceDto>>> GetLowStockItemsAsync()
    {
        var allBalances = await GetStockBalanceAsync();
        var lowStock = allBalances.Data!
            .Where(b => b.StockStatus is "Low Stock" or "Out of Stock")
            .ToList();

        return ApiResponse<List<StockBalanceDto>>.SuccessResponse(lowStock);
    }

    public async Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync()
    {
        var items = await _itemRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();
        var suppliers = await _supplierRepository.GetAllAsync();
        var stockIns = await _stockInRepository.GetAllAsync();
        var stockOuts = await _stockOutRepository.GetAllAsync();

        int lowStockCount = 0, outOfStockCount = 0;
        foreach (var item in items)
        {
            var totalIn = stockIns.Where(s => s.ItemId == item.ItemId).Sum(s => s.Quantity);
            var totalOut = stockOuts.Where(s => s.ItemId == item.ItemId).Sum(s => s.Quantity);
            var balance = totalIn - totalOut;
            var status = GetStockStatus(balance, item.ReorderLevel);
            if (status == "Out of Stock") outOfStockCount++;
            else if (status == "Low Stock") lowStockCount++;
        }

        return ApiResponse<DashboardStatsDto>.SuccessResponse(new DashboardStatsDto
        {
            TotalItems = items.Count,
            TotalCategories = categories.Count,
            TotalSuppliers = suppliers.Count,
            LowStockItems = lowStockCount,
            OutOfStockItems = outOfStockCount,
            TotalStockInTransactions = stockIns.Count,
            TotalStockOutTransactions = stockOuts.Count
        });
    }

    // ─── Private Helpers ───────────────────────────────────────────────────────

    private static string GetStockStatus(int currentBalance, int reorderLevel)
    {
        if (currentBalance <= 0) return "Out of Stock";
        if (currentBalance <= reorderLevel) return "Low Stock";
        return "Good Stock";
    }
}
