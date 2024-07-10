using System.Linq.Expressions;
using Domain.Common;

namespace Application.Repositories.Common;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<T?> GetByQueryAsync(Expression<Func<T, bool>> query, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<int> CountByQueryAsync(Expression<Func<T, bool>> query, CancellationToken cancellationToken);
    Task<PaginatedResult<T>> GetAllAsync(BaseFilter filter, CancellationToken cancellationToken);

    Task<PaginatedResult<T>> GetAllSortedByQueryAsync(
        BaseFilter filter,
        Expression<Func<T, object>> query,
        CancellationToken cancellationToken);

    Task<PaginatedResult<T>> GetAllAsync(
        Expression<Func<T, bool>> query,
        BaseFilter filter,
        CancellationToken cancellationToken);

    Task<int> AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}