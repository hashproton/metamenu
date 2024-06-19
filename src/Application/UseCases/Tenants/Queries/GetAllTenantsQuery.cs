using Application.Repositories.Common;
using Application.Services;

namespace Application.UseCases.Tenants.Queries;

public record GetAllTenantsQuery(
    PaginatedQuery PaginatedQuery) : IRequest<PaginatedResult<GetAllTenantsQueryResponse>>;

public class GetAllTenantsQueryHandler(
    ILogger logger,
    ITenantRepository tenantRepository)
    : IRequestHandler<GetAllTenantsQuery, PaginatedResult<GetAllTenantsQueryResponse>>
{
    public async Task<PaginatedResult<GetAllTenantsQueryResponse>> Handle(
        GetAllTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await tenantRepository.GetAllAsync(request.PaginatedQuery, cancellationToken);

        logger.LogInformation("Retrieving all tenants");

        return result.Map(t => new GetAllTenantsQueryResponse(t.Id, t.Name));
    }
}

public record GetAllTenantsQueryResponse(
    int Id,
    string Name);