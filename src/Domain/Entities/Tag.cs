using Domain.Common;

namespace Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = default!;

    public TagGroup TagGroup { get; set; } = default!;
    public int TagGroupId { get; set; }
}