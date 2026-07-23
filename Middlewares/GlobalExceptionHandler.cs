using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Middlewares;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            InvalidOperationException => (StatusCodes.Status409Conflict, "Operation not valid"),
            OperationCanceledException => (499, "Request was cancelled"),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Forbidden access to this resource"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred"),
        };

        logger.LogError(exception, "Request failed with {StatusCode}", statusCode);

        if (environment.IsDevelopment() && statusCode == 500)
            title = exception.Message;

        httpContext.Response.StatusCode = statusCode;

        return await httpContext.RequestServices
            .GetRequiredService<IProblemDetailsService>()
            .TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Type = $"https://httpstatuses.io/{statusCode}"
                }
            });
    }
}