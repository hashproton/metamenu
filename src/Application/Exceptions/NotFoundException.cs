using Application.Exceptions.Common;

namespace Application.Exceptions;

public class NotFoundException(
    string entity,
    int identifier) : Exception
{
    public string Entity { get; } = entity;

    public int Identifier { get; } = identifier;

    public static string Code => ExceptionCodes.NotFound;

    public override string Message => $"{Entity} with ID {Identifier} was not found.";
}