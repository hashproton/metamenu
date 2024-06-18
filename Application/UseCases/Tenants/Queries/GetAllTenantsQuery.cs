using Application.Repositories;
using Application.Repositories.Common;
using MediatR;

namespace Application.UseCases.Tenants.Queries;

public record GetAllTenantsQuery(
    PaginatedQuery PaginatedQuery) : IRequest<PaginatedResult<GetAllTenantsQueryResponse>>;

public class GetAllTenantsQueryHandler(
    ITenantRepository tenantRepository) : IRequestHandler<GetAllTenantsQuery, PaginatedResult<GetAllTenantsQueryResponse>>
{
    public async Task<PaginatedResult<GetAllTenantsQueryResponse>> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
    {
        var result = await tenantRepository.GetAllAsync(request.PaginatedQuery, cancellationToken);
        
        return result.Map(t => new GetAllTenantsQueryResponse(t.Id, t.Name));
    }
}

public record GetAllTenantsQueryResponse(
    Guid Id,
    string Name);