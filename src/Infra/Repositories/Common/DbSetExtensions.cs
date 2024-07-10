using Application.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Common;

public static class DbSetExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> query,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

        var items = await query
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items,
            totalItems,
            totalPages,
            filter.PageNumber,
            filter.PageSize);
    }
}
