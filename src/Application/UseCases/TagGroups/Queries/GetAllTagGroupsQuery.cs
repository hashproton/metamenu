using Application.Repositories.Common;
using Application.Services;
using Application.UseCases.TagGroups.Queries.Common;

namespace Application.UseCases.TagGroups.Queries;

public record GetAllTagGroupsQuery(
    int TenantId,
    BaseFilter Filter) : IRequest<Result<PaginatedResult<TagGroupQueryResponse>>>;

public class GetAllTagGroupsQueryHandler(
    ILogger logger,
    ITenantRepository tenantRepository,
    ITagGroupRepository tagGroupRepository)
    : IRequestHandler<GetAllTagGroupsQuery, Result<PaginatedResult<TagGroupQueryResponse>>>
{
    public async Task<Result<PaginatedResult<TagGroupQueryResponse>>> Handle(
        GetAllTagGroupsQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            return Result.Failure<PaginatedResult<TagGroupQueryResponse>>(TenantErrors.TenantNotFound);
        }

        var result = await tagGroupRepository.GetAllAsync(request.Filter, cancellationToken);

        logger.LogInformation($"Retrieving all tag groups for tenant {tenant.Name}.");
        
        return Result
            .Success(result.Map(t => t.ToQueryResponse()));
    }
}
