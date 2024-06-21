using Domain.Common;

namespace Domain.Entities;

public enum TenantStatus
{
    Active = 0,
    Inactive = 1,
    Demo = 2
}

public class Tenant : BaseEntity
{
    public string Name { get; set; } = default!;

    public TenantStatus Status { get; set; }
}