using System.Linq.Expressions;
using Application.Repositories;
using Application.Repositories.Common;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Common;

internal class GenericRepository<T>(
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

    public Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return context.Set<T>().CountAsync(cancellationToken);
    }

    public Task<int> CountByQueryAsync(Expression<Func<T, bool>> query, CancellationToken cancellationToken)
    {
        return context.Set<T>().CountAsync(query, cancellationToken);
    }

    public async Task<PaginatedResult<T>> GetAllAsync(
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        return await context.Set<T>()
            .ApplyFilter(filter)
            .ToSort(filter)
            .ToPaginatedResultAsync(filter, cancellationToken);
    }

    public async Task<PaginatedResult<T>> GetAllSortedByQueryAsync(
        BaseFilter filter,
        Expression<Func<T, object>> query,
        CancellationToken cancellationToken)
    {
        return await context.Set<T>()
            .OrderBy(query)
            .ToPaginatedResultAsync(filter, cancellationToken);
    }

    public async Task<PaginatedResult<T>> GetAllAsync(
        Expression<Func<T, bool>> query,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        return await context.Set<T>()
            .Where(query)
            .ToPaginatedResultAsync(filter, cancellationToken);
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