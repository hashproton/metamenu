using System.Linq.Expressions;
using Application.Repositories.Common;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Common;

public class GenericRepository<T>(
    AppDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Set<T>().FindAsync([id], cancellationToken);
    }

    public Task<T?> GetByQueryAsync(Expression<Func<T, bool>> query, CancellationToken cancellationToken)
    {
        return context.Set<T>().FirstOrDefaultAsync(query, cancellationToken);
    }

    public async Task<PaginatedResult<T>> GetAllAsync(PaginatedQuery paginatedQuery,
        CancellationToken cancellationToken)
    {
        var totalItems = await context.Set<T>().CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)paginatedQuery.PageSize);

        return new PaginatedResult<T>(
            await context.Set<T>()
                .Skip(paginatedQuery.PageSize * (paginatedQuery.PageNumber - 1))
                .Take(paginatedQuery.PageSize)
                .ToListAsync(cancellationToken),
            totalItems,
            totalPages,
            paginatedQuery.PageNumber,
            paginatedQuery.PageSize);
    }

    public async Task<int> AddAsync(T entity, CancellationToken cancellationToken)
    {
        var entry = context.Set<T>().Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entry.Entity.Id;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Update(entity);

        await context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Remove(entity);

        return context.SaveChangesAsync(cancellationToken);
    }
}