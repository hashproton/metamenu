namespace Application.Repositories.Common;

public class BaseFilter
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? SortTerm { get; set; }

    public string? FilterTerm { get; set; }
}