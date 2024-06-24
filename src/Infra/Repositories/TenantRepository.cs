using Application.Repositories;
using Domain.Entities;
using Infra.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

internal sealed class TenantRepository(AppDbContext context) : GenericRepository<Tenant>(context), ITenantRepository
{
    public Task<Tenant?> GetTenantByNameAsync(string name, CancellationToken cancellationToken)
    {
        return context.Tenants.FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
    }

    public Task<List<TenantMetadata>> GetTenantsStatusAsync(CancellationToken cancellationToken)
    {
        return context.Tenants
            .GroupBy(t => t.Status)
            .Select(g => new TenantMetadata
            {
                Status = g.Key,
                Count = g.Count()
            }).ToListAsync(cancellationToken);
    }
}