using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using ILogger = Application.Services.ILogger;

namespace Api.Middlewares;

internal sealed class GlobalExceptionHandler(
    ILogger logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception.Message);
        
        var result = new ObjectResult("Internal server error")
        {
            ContentTypes = [MediaTypeNames.Application.Json],
            StatusCode = StatusCodes.Status500InternalServerError
        };
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}