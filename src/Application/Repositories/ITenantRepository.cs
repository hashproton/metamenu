using Application.Repositories.Common;

namespace Application.Repositories;

public class TenantFilter : BaseFilter
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public TenantStatus? Status { get; set; }
}

public interface ITenantRepository : IGenericRepository<Tenant>
{
    public Task<Tenant?> GetTenantByNameAsync(string name, CancellationToken cancellationToken);

    Task<List<TenantMetadata>> GetTenantsStatusAsync(CancellationToken cancellationToken);
}

public class TenantMetadata
{
    public TenantStatus Status { get; set; }
    public int Count { get; set; }
}