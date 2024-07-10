using Application.Attributes.Authorize;
using Application.Models.Auth;
using Application.Repositories.Common;
using Application.Services;
using Application.UseCases.Tenants.Queries.Common;

namespace Application.UseCases.Tenants.Queries;

[Authorize(Role.Admin)]
public record GetAllTenantsQuery(
    TenantFilter Filter) : IRequest<Result<PaginatedResult<TenantQueryResponse>>>;

public class GetAllTenantsQueryHandler(
    ILogger logger,
    ITenantRepository tenantRepository)
    : IRequestHandler<GetAllTenantsQuery, Result<PaginatedResult<TenantQueryResponse>>>
{
    public async Task<Result<PaginatedResult<TenantQueryResponse>>> Handle(
        GetAllTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await tenantRepository.GetAllAsync(
            request.Filter,
            cancellationToken);

        logger.LogInformation("Retrieving all tenants");

        return Result
            .Success(result.Map(t => t.ToQueryResponse()));
    }
}