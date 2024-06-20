namespace Application.Errors.Common;

public class Result<T> : Result
{
    public T Value { get; }

    internal Result(T value)
    {
        Value = value;
    }

    internal Result(Error error) : base(error)
    {
        Value = default!;
    }
}

public class Result
{
    public Error? Error { get; }

    public bool IsSuccess => Error is null;

    protected Result(Error? error = null)
    {
        Error = error;
    }

    public static Result Failure(Error error) => new(error);

    public static Result<T> Failure<T>(Error error) => new(error);

    public static Result<T> Success<T>(T value) where T : notnull => new Result<T>(value);

    public static Result Success() => new();
}