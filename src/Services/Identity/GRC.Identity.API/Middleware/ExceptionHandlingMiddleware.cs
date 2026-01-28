using GRC.BuildingBlocks.Domain.Exceptions;
using GRC.Identity.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace GRC.Identity.API.Middleware;

/// <summary>
/// Middleware para manejo centralizado de excepciones
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An error occurred while processing your request";
        var details = exception.Message;

        switch (exception)
        {            
            case DomainException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Domain validation error";
                break;

            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Invalid operation";
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Unauthorized access";
                break;

            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = "Resource not found";
                break;
        }

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message,
            Details = details,
            Timestamp = DateTime.UtcNow
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Details { get; set; }
    public DateTime Timestamp { get; set; }
}
//public static class ExceptionHandlingMiddlewareExtensions
//{
//    public static IApplicationBuilder UseExceptionHandlingMiddleware(
//        this IApplicationBuilder builder)
//    {
//        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
//    }
//}