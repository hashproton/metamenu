using Application.Repositories;
using Domain.Entities;
using Infra.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class TagGroupRepository(AppDbContext context) : GenericRepository<TagGroup>(context), ITagGroupRepository
{
    public Task<TagGroup?> GetTagGroupByNameAsync(int tenantId, string tagGroupName, CancellationToken cancellationToken)
    {
        return context.TagGroups
            .Include(tg => tg.Tenant)
            .FirstOrDefaultAsync(tg => tg.Tenant.Id == tenantId && tg.Name == tagGroupName, cancellationToken);
    }
}