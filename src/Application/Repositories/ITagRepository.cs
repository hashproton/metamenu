using Application.Repositories.Common;

namespace Application.Repositories;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<Tag?> GetTagByNameAsync(
        int tagGroupId,
        string name,
        CancellationToken cancellationToken);
    
    Task<PaginatedResult<Tag>> GetAllAsync(
        int tenantId,
        PaginatedQuery paginatedQuery,
        CancellationToken cancellationToken);
}