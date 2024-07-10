namespace Application.Repositories.Common;

public class PaginatedQuery
{
    public PaginatedQuery(
        int pageNumber,
        int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    
    private const int MaxPageSize = 50;
    
    private int _pageNumber;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    private readonly int _pageSize;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}