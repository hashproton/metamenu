namespace Application.UseCases.Tenants.Queries.Common;

public record TenantQueryResponse(
    int Id,
    string Name,
    TenantStatus Status);
    
public static class TenantQueryResponseExtensions
{
    public static TenantQueryResponse ToQueryResponse(this Tenant tenant)
    {
        return new TenantQueryResponse(tenant.Id, tenant.Name, tenant.Status);
    }
}