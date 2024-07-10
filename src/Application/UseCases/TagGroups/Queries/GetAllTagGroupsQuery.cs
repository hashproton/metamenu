using Application.Repositories.Common;
using Application.Services;

namespace Application.UseCases.TagGroups.Queries;

public record GetAllTagGroupsQuery(
    int TenantId,
    BaseFilter Filter) : IRequest<PaginatedResult<GetAllTagGroupsQueryResponse>>;

public class GetAllTagGroupsQueryHandler(
    ILogger logger,
    ITenantRepository tenantRepository,
    ITagGroupRepository tagGroupRepository)
    : IRequestHandler<GetAllTagGroupsQuery, PaginatedResult<GetAllTagGroupsQueryResponse>>
{
    public async Task<PaginatedResult<GetAllTagGroupsQueryResponse>> Handle(
        GetAllTagGroupsQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            throw new NotFoundException(nameof(Tenant), request.TenantId);
        }

        var result = await tagGroupRepository.GetAllAsync(request.Filter, cancellationToken);

        logger.LogInformation($"Retrieving all tag groups for tenant {tenant.Name}.");

        return result.Map(t => new GetAllTagGroupsQueryResponse(t.Id, t.Name));
    }
}

public record GetAllTagGroupsQueryResponse(
    int Id,
    string Name);