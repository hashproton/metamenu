using Domain.Common;

namespace Domain.Entities;

public class TagGroup : BaseEntity
{
    public string Name { get; set; } = default!;

    public Tenant Tenant { get; set; } = default!;
    public int TenantId { get; set; }

    public List<Tag> Tags { get; set; } = new();
}