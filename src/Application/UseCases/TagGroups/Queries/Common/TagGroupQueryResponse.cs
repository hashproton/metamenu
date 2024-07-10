namespace Application.UseCases.TagGroups.Queries.Common;

public record TagGroupQueryResponse(
    int Id,
    string Name,
    int TenantId);
    
public static class TagGroupQueryResponseExtensions
{
    public static TagGroupQueryResponse ToQueryResponse(this TagGroup tagGroup)
    {
        return new TagGroupQueryResponse(tagGroup.Id, tagGroup.Name, tagGroup.TenantId);
    }
}