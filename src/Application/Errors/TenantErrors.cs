namespace Application.Errors;

public static class TenantErrors
{
    public static readonly Error TenantAlreadyExists = Error.Conflict("tenant_already_exists", "Tenant with the same name already exists");
    
    public static readonly Error TenantNotFound = Error.NotFound("tenant_not_found", "Tenant not found");
}
