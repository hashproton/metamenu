using Domain.Common;

namespace Domain.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = default!;
}