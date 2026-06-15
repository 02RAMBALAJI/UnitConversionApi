using System.Net;
using System.Text.Json;
using UnitConversionApi.Models;

namespace UnitConversionApi.Middleware;

/// <summary>
/// Global exception handler that maps domain exceptions to appropriate HTTP responses.
/// Keeps controllers free of try/catch boilerplate.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next   = next;
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
            _logger.LogError(ex, "Unhandled exception on {Method} {Path}: {Message}",
                context.Request.Method, context.Request.Path, ex.Message);

            await WriteErrorResponseAsync(context, ex);
        }
    }

    private static Task WriteErrorResponseAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            ArgumentException e          => (HttpStatusCode.BadRequest,           e.Message),
            InvalidOperationException e  => (HttpStatusCode.UnprocessableEntity,  e.Message),
            NotSupportedException e      => (HttpStatusCode.BadRequest,           e.Message),
            _                            => (HttpStatusCode.InternalServerError,  "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = (int)statusCode;

        var body = JsonSerializer.Serialize(
            ApiResponse<object>.Fail(message), JsonOptions);

        return context.Response.WriteAsync(body);
    }
}
