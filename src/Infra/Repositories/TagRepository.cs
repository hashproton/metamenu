using Application.Repositories;
using Application.Repositories.Common;
using Domain.Entities;
using Infra.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

internal sealed class TagRepository(AppDbContext context) : GenericRepository<Tag>(context), ITagRepository
{
    private readonly AppDbContext _context = context;

    public Task<Tag?> GetTagByNameAsync(
        int tenantId,
        string tagName,
        CancellationToken cancellationToken)
    {
        return _context.Tags
            .Include(t => t.TagGroup)
            .ThenInclude(tg => tg.Tenant)
            .FirstOrDefaultAsync(t => t.TagGroup.Tenant.Id == tenantId && t.Name == tagName, cancellationToken);
    }

    public Task<PaginatedResult<Tag>> GetAllAsync(
        int tenantId,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        return _context.Tags
            .Include(t => t.TagGroup)
            .ThenInclude(tg => tg.Tenant)
            .Where(t => t.TagGroup.Tenant.Id == tenantId)
            .ToPaginatedResultAsync(filter, cancellationToken);
    }
}