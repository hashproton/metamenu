using Domain.Common;

namespace Application.Repositories.Common;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedResult<T>> GetAllAsync(PaginatedQuery paginatedQuery, CancellationToken cancellationToken);
    Task<Guid> AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}