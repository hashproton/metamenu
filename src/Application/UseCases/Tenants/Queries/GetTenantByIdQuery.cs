using Application.Services;

namespace Application.UseCases.Tenants.Queries;

public record GetTenantByIdQuery(
    int Id) : IRequest<Result<GetTenantByIdQueryResponse>>;

public class GetTenantByIdQueryHandler(
    ILogger logger,
    ITenantRepository tenantRepository)
    : IRequestHandler<GetTenantByIdQuery, Result<GetTenantByIdQueryResponse>>
{
    public async Task<Result<GetTenantByIdQueryResponse>> Handle(
        GetTenantByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tenant is null)
        {
            return Result.Failure<GetTenantByIdQueryResponse>(TenantErrors.TenantNotFound);
        }

        logger.LogInformation($"Retrieving tenant {tenant.Id}");

        return Result.Success(new GetTenantByIdQueryResponse(tenant.Id, tenant.Name));
    }
}

public record GetTenantByIdQueryResponse(
    int Id,
    string Name);