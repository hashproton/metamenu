namespace Application.Errors;

public static class TenantErrors
{
    public static readonly Error TenantAlreadyExists = Error.Conflict("tenant_already_exists", "Tenant with the same name already exists");
    
    public static readonly Error TenantNotFound = Error.NotFound("tenant_not_found", "Tenant not found");
}

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = Error.Unauthorized("invalid_credentials", "Invalid credentials");
    
    public static readonly Error Unauthorized = Error.Unauthorized("unauthorized", "Unauthorized");

    public static Error Forbidden => Error.Forbidden("forbidden", "You are not allowed to perform this action");
}