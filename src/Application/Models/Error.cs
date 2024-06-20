namespace Application.Models;

public enum ErrorType
{
    Failure,
    Validation,
    NotFound,
    Conflict,
}

public class Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);
    
    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);
    
    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
    
    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
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