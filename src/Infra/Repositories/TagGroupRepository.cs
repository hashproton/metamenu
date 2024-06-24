using Application.Repositories;
using Domain.Entities;
using Infra.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

internal sealed class TagGroupRepository(AppDbContext context)
    : GenericRepository<TagGroup>(context), ITagGroupRepository
{
    private readonly AppDbContext _context = context;

    public Task<TagGroup?> GetTagGroupByNameAsync(
        int tenantId,
        string tagGroupName,
        CancellationToken cancellationToken)
    {
        return _context.TagGroups
            .Include(tg => tg.Tenant)
            .FirstOrDefaultAsync(tg => tg.Tenant.Id == tenantId && tg.Name == tagGroupName, cancellationToken);
    }
}