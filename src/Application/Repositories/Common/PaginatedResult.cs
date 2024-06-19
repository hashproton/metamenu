using Domain.Common;

namespace Application.Repositories.Common;

public record PaginatedResult<T>(
    IEnumerable<T> Items,
    int TotalItems,
    int TotalPages,
    int PageNumber,
    int PageSize)
{
    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}

public static class PaginatedResultExtensions
{
    public static PaginatedResult<TDestination> Map<T, TDestination>(
        this PaginatedResult<T> paginatedResult,
        Func<T, TDestination> map) where T : BaseEntity
    {
        return new PaginatedResult<TDestination>(
            paginatedResult.Items.Select(map),
            paginatedResult.TotalItems,
            paginatedResult.TotalPages,
            paginatedResult.PageNumber,
            paginatedResult.PageSize);
    }
}