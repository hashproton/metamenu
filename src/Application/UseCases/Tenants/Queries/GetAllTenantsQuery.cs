using Application.Attributes.Authorize;
using Application.Repositories.Common;
using Application.Services;
using Application.UseCases.Tenants.Commands;
using Application.UseCases.Tenants.Queries.Common;

namespace Application.UseCases.Tenants.Queries;

[Authorize(Role.SuperAdmin)]
public record GetAllTenantsQuery(
    PaginatedQuery PaginatedQuery) : IRequest<Result<PaginatedResult<TenantQueryResponse>>>;

public class GetAllTenantsQueryHandler(
    AuthContext authContext,
    ILogger logger,
    ITenantRepository tenantRepository)
    : IRequestHandler<GetAllTenantsQuery, Result<PaginatedResult<TenantQueryResponse>>>
{
    public async Task<Result<PaginatedResult<TenantQueryResponse>>> Handle(
        GetAllTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await tenantRepository.GetAllSortedByQueryAsync(
            request.PaginatedQuery,
            t => t.Id,
            cancellationToken);

        logger.LogInformation("Retrieving all tenants");

        return Result
            .Success(result.Map(t => t.ToQueryResponse()));
    }
}