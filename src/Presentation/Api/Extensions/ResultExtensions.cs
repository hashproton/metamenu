using System.Net.Mime;
using Application.Errors.Common;

namespace Api.Extensions;

public static class ResultExtensions
{
    public static ProblemDetails ToProblemDetails(this Error error)
    {
        return new ProblemDetails
        {
            Title = GetTitle(error!.Type),
            Status = GetStatusCode(error.Type),
            Detail = error.Message,
            Extensions =
            {
                { "errors", new[] { error } }
            },
        };
    }

    public static ActionResult ToActionResult(this Result result)
    {
        return new ObjectResult(GetTitle(result.Error!.Type))
        {
            ContentTypes = [MediaTypeNames.Application.Json],
            StatusCode = GetStatusCode(result.Error.Type),
            Value = new Dictionary<string, object>()
            {
                { "errors", new[] { result.Error } }
            }
        };
    }

    private static int GetStatusCode(ErrorType type)
    {
        return type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetTitle(ErrorType type)
    {
        return type switch
        {
            ErrorType.Validation => "Validation error",
            ErrorType.NotFound => "Resource not found",
            ErrorType.Conflict => "Conflict",
            ErrorType.Forbidden => "Forbidden",
            ErrorType.Unauthorized => "Unauthorized",
            _ => "Internal server error"
        };
    }
}