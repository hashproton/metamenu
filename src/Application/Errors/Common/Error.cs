namespace Application.Errors.Common;

public class Error
{
    private bool Equals(Error other)
    {
        return Code == other.Code && Message == other.Message && Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((Error)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message, (int)Type);
    }

    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    public Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);
    
    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);
    
    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
    
    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
    
    public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);
    
    public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);
}