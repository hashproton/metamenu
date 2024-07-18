using System.Diagnostics.CodeAnalysis;
using Api.Extensions;
using Application.Exceptions;
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

        ProblemDetails result;
        if (IsResultException(exception, out var resultErrorException))
        {
            result = resultErrorException.Error.ToProblemDetails();
        }
        else
        {
            result = new ProblemDetails
            {
                Title = "Internal server error",
                Status = StatusCodes.Status500InternalServerError,
            };
        }

        httpContext.Response.StatusCode = result.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }

    private static bool IsResultException(
        Exception exception,
        [NotNullWhen(true)] out ResultErrorException? resultException)
    {
        while (exception is not null)
        {
            if (exception is ResultErrorException resultErrorException)
            {
                resultException = resultErrorException;
                return true;
            }

            if (exception.InnerException is null)
            {
                resultException = null;
                return false;
            }

            exception = exception.InnerException;
        }

        resultException = null;
        return false;
    }
}
