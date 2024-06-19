using Application.Services;

namespace Application.UseCases.Tenants.Queries;

public record GetTenantByIdQuery(
    int Id) : IRequest<GetTenantByIdQueryResponse>;

public class GetTenantByIdQueryHandler(
    ILogger logger,
    ITenantRepository tenantRepository)
    : IRequestHandler<GetTenantByIdQuery, GetTenantByIdQueryResponse>
{
    public async Task<GetTenantByIdQueryResponse> Handle(GetTenantByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tenant is null) throw new NotFoundException(nameof(Tenant), request.Id);

        logger.LogInformation($"Retrieving tenant {tenant.Id}");

        return new GetTenantByIdQueryResponse(tenant.Id, tenant.Name);
    }
}

public record GetTenantByIdQueryResponse(
    int Id,
    string Name);