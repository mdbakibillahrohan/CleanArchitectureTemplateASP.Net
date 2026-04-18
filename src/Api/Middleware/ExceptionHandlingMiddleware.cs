using System.ComponentModel.DataAnnotations;
using Domain.Abstractions;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlingMiddleware> logger, 
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            // Log the full exception locally on your Mac for debugging
            _logger.LogError(exception, "Exception: {Message}", exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Determine Status Code
        int statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        // Determine the Message
        string displayMessage;
        
        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            // For 500 errors, only show the real message if we are in Development
            displayMessage = _env.IsDevelopment() 
                ? $"Internal Server Error: {exception.Message}" 
                : "An internal server error occurred.";
        }
        else
        {
            // For 400, 401, 404 etc., the message is usually a friendly business rule 
            // (e.g., "User not found"), so we show it in all environments.
            displayMessage = exception.Message;
        }

        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = displayMessage,
            StatusCode = statusCode,
            // Include StackTrace only in Dev for even easier debugging
            Data = _env.IsDevelopment() ? exception.StackTrace : null 
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}