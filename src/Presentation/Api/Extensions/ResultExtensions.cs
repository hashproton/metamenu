using System.Net.Mime;
using Application.Models;

namespace Api.Extensions;

public static class ResultExtensions
{
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
            _ => "Internal server error"
        };
    }
}