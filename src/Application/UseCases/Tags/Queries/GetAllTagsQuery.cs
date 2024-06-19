using Application.Repositories.Common;
using Application.Services;

namespace Application.UseCases.Tags.Queries;

public record GetAllTagsQuery(
    int TenantId,
    PaginatedQuery PaginatedQuery) : IRequest<PaginatedResult<GetAllTagsQueryResponse>>;

public class GetAllTagsQueryHandler(
    ILogger logger,
    ITenantRepository tenantRepository,
    ITagRepository tagRepository) : IRequestHandler<GetAllTagsQuery, PaginatedResult<GetAllTagsQueryResponse>>
{
    public async Task<PaginatedResult<GetAllTagsQueryResponse>> Handle(
        GetAllTagsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantId = await tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenantId is null)
        {
            throw new NotFoundException(nameof(Tenant), request.TenantId);
        }

        var tags = await tagRepository.GetAllAsync(request.TenantId, request.PaginatedQuery, cancellationToken);

        logger.LogInformation($"Retrieved {tags.Items.Count()} Tags.");

        return tags.Map(t => new GetAllTagsQueryResponse(t.Id, t.Name, t.TagGroup.Id, t.TagGroup.Name));
    }
}

public record GetAllTagsQueryResponse(
    int Id,
    string Name,
    int TagGroupId,
    string TagGroupName);