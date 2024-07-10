using Domain.Common;

namespace Application.Repositories.Common;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<int> CountAsync(CancellationToken cancellationToken);

    Task<PaginatedResult<T>> GetAllAsync<TFilter>(
        TFilter filter,
        CancellationToken cancellationToken) where TFilter : BaseFilter;

    Task<int> AddAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}