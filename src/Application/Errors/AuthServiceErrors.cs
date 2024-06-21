namespace Application.Errors;

public static class AuthServiceErrors
{
    public static readonly Error InvalidResponse = Error.Failure("invalid_response", "Invalid response from auth service");
}