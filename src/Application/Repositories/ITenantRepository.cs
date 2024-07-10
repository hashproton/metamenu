using Application.Repositories.Common;

namespace Application.Repositories;

public enum Operation
{
    Contains,
    Equals
}

public class BaseFilter
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? SortByField { get; set; }

    public SortDirection SortDirection { get; set; }

    public Operation FilterOperation { get; set; }

    public string? FilterValue { get; set; }

    public string? FilterField { get; set; }
}

public enum SortDirection
{
    Asc,
    Desc
}

public class TenantFilter : BaseFilter
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public TenantStatus? Status { get; set; }
}

public interface ITenantRepository : IGenericRepository<Tenant>
{
    public Task<Tenant?> GetTenantByNameAsync(string name, CancellationToken cancellationToken);

    Task<List<TenantMetadata>> GetTenantsStatusAsync(CancellationToken cancellationToken);
}

public class TenantMetadata
{
    public TenantStatus Status { get; set; }
    public int Count { get; set; }
}