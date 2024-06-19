using Application.Repositories.Common;

namespace Application.Repositories;

public interface ITagGroupRepository : IGenericRepository<TagGroup>
{
    Task<TagGroup?> GetTagGroupByNameAsync(int tenantId, string tagGroupName, CancellationToken cancellationToken);
}