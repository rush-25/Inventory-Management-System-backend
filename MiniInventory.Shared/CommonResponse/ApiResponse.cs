namespace MiniInventory.Shared.CommonResponse;

/// <summary>
/// Generic API response wrapper used across all endpoints.
/// </summary>
/// <typeparam name="T">The type of data payload.</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully.")
        => new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> FailureResponse(string message, List<string>? errors = null)
        => new() { Success = false, Message = message, Errors = errors };
}

/// <summary>
/// Non-generic wrapper for operations that return no data payload.
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }

    public static ApiResponse SuccessResponse(string message = "Operation completed successfully.")
        => new() { Success = true, Message = message };

    public static ApiResponse FailureResponse(string message, List<string>? errors = null)
        => new() { Success = false, Message = message, Errors = errors };
}
