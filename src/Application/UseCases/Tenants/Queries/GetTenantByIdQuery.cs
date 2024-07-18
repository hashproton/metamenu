using Application.Services;
using Application.UseCases.Tenants.Queries.Common;

namespace Application.UseCases.Tenants.Queries;

public record GetTenantByIdQuery(
    int TenantId) : IRequest<Result<TenantQueryResponse>>;

public class GetTenantByIdQueryHandler(
    ILogger logger,
    ITenantRepository tenantRepository)
    : IRequestHandler<GetTenantByIdQuery, Result<TenantQueryResponse>>
{
    public async Task<Result<TenantQueryResponse>> Handle(
        GetTenantByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            return Result.Failure<TenantQueryResponse>(TenantErrors.TenantNotFound);
        }

        logger.LogInformation($"Retrieving tenant {tenant.Id}");

        return Result.Success(tenant.ToQueryResponse());
    }
}