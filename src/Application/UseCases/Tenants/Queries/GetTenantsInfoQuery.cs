using Application.Services;

namespace Application.UseCases.Tenants.Queries;

public record GetTenantsInfoQuery : IRequest<Result<GetTenantsInfoQueryResponse>>;

public class GetTenantsInfoQueryHandler(
    ILogger logger,
    ITenantRepository tenantRepository)
    : IRequestHandler<GetTenantsInfoQuery, Result<GetTenantsInfoQueryResponse>>
{
    public async Task<Result<GetTenantsInfoQueryResponse>> Handle(
        GetTenantsInfoQuery request,
        CancellationToken cancellationToken)
    {
        var tenants = await tenantRepository.GetTenantsStatusAsync(cancellationToken);
        var count = await tenantRepository.CountAsync(cancellationToken);
        
        logger.LogInformation("Retrieving tenants info");

        return Result.Success(new GetTenantsInfoQueryResponse(
            tenants.Single(t => t.Status == TenantStatus.Active).Count,
            tenants.Single(t => t.Status == TenantStatus.Inactive).Count,
            tenants.Single(t => t.Status == TenantStatus.Demo).Count,
            count));
    }
}

public record GetTenantsInfoQueryResponse(
    int Active,
    int Inactive,
    int Demo,
    int Total);