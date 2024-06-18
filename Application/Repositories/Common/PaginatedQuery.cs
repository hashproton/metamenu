namespace Application.Repositories.Common;

public record PaginatedQuery(
    int PageNumber,
    int PageSize);