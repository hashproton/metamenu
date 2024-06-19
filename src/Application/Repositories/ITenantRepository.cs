using Application.Repositories.Common;

namespace Application.Repositories;

public interface ITenantRepository : IGenericRepository<Tenant>
{
    public Task<Tenant?> GetTenantByNameAsync(string name, CancellationToken cancellationToken);
}