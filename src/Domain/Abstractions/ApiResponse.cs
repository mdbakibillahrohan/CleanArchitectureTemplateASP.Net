using Microsoft.AspNetCore.Http;

namespace Domain.Abstractions;

public class ApiResponse<TValue>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
    public TValue? Data { get; set; }

    public static ApiResponse<TValue> Ok(TValue value, string? message = null, int statusCode = StatusCodes.Status200OK)
    {
        return new ApiResponse<TValue>
        {
            Success = true,
            Message = message,
            StatusCode = statusCode,
            Data = value
        };
    }

    public static ApiResponse<TValue> Fail(string message, int statusCode = StatusCodes.Status400BadRequest)
    {
        return new ApiResponse<TValue>
        {
            Success = false,
            Message = message,
            StatusCode = statusCode,
            Data = default
        };
    }

    public static ApiResponse<TValue> NotFound(string message = "Resource not found")
    {
        return new ApiResponse<TValue>
        {
            Success = false,
            Message = message,
            StatusCode = StatusCodes.Status404NotFound,
            Data = default
        };
    }

    public static ApiResponse<TValue> Unauthorized(string message = "Unauthorized")
    {
        return new ApiResponse<TValue>
        {
            Success = false,
            Message = message,
            StatusCode = StatusCodes.Status401Unauthorized,
            Data = default
        };
    }

    public static ApiResponse<TValue> InternalError(string message = "Internal server error")
    {
        return new ApiResponse<TValue>
        {
            Success = false,
            Message = message,
            StatusCode = StatusCodes.Status500InternalServerError,
            Data = default
        };
    }

    public static ApiResponse<TValue> BadRequest(string message = "Bad request")
    {
        return new ApiResponse<TValue>
        {
            Success = false,
            Message = message,
            StatusCode = StatusCodes.Status400BadRequest,
            Data = default
        };
    }
}

