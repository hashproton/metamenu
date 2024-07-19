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
    public IEnumerable<Error>? Errors { get; }

    public bool IsSuccess => Errors is null || !Errors.Any(); 

    protected Result(Error? error = null)
    {
        Errors = error is null ? null : new[] { error };
    }

    private Result(IEnumerable<Error>? errors)
    {
        Errors = errors;
    }

    public static Result Failure(Error error) => new(error);
    
    public static Result Failure(IEnumerable<Error> errors) => new(errors); 

    public static Result<T> Failure<T>(Error error) => new(error);

    public static Result<T> Success<T>(T value) where T : notnull => new(value);

    public static Result Success() => new();
}