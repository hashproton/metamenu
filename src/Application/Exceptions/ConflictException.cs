using Application.Exceptions.Common;

namespace Application.Exceptions;

public class ConflictException(
    string message) : Exception
{
    public static string Code => ExceptionCodes.Conflict;

    public override string Message => message;
}