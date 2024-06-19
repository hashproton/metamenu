using Application.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Common;

public static class DbSetExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> query,
        PaginatedQuery paginatedQuery,
        CancellationToken cancellationToken)
    {
        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)paginatedQuery.PageSize);

        var items = await query
            .Skip(paginatedQuery.PageSize * (paginatedQuery.PageNumber - 1))
            .Take(paginatedQuery.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items, totalItems, totalPages, paginatedQuery.PageNumber, paginatedQuery.PageSize);
    }
}