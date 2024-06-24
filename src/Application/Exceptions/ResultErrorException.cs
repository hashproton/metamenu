namespace Application.Exceptions;

public class ResultErrorException(
    Error error) : Exception
{
    public Error Error { get; } = error;
}