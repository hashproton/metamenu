using System.Diagnostics.CodeAnalysis;
using Api.Extensions;
using Application.Exceptions;
using Application.Models;
using Application.Services.AuthService;
using Application.UseCases.Tenants.Commands;
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

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }

    private static bool IsResultException(Exception exception, [NotNullWhen(true)] out ResultErrorException? resultException)
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

internal sealed class SetAuthContextMiddleware(
    AuthContext authContext,
    IAuthService authService) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/api/auth"))
        {
            await next(context);
            return;
        }

        var authorization = context.Request.Headers["Authorization"];
        var refreshToken = context.Request.Headers["RefreshToken"];

        if (string.IsNullOrWhiteSpace(authorization))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync("Authorization header is missing", context.RequestAborted);

            return;
        }
        
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync("RefreshToken header is missing", context.RequestAborted);

            return;
        }

        var user = await authService.GetMeAsync(authorization!, refreshToken!, context.RequestAborted);
        if (!user.IsSuccess)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(user.Error, context.RequestAborted);

            return;
        }

        authContext.UserId = user.Value.Id;
        authContext.Roles = user.Value.Roles;
        authContext.TenantIds = user.Value.TenantIds;

        await next(context);
    }
}