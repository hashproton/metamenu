using Application.Repositories;
using Domain.Entities;
using Infra.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class TenantRepository(AppDbContext context) : GenericRepository<Tenant>(context), ITenantRepository
{
    public Task<Tenant?> GetTenantByNameAsync(string name, CancellationToken cancellationToken)
    {
        return context.Tenants.FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
    }
}