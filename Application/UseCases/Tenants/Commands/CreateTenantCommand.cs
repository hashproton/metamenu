using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Tenants.Commands;

public record CreateTenantCommand(
    string Name) : IRequest<Guid>;

public class CreateTenantCommandHandler(ITenantRepository tenantRepository) : IRequestHandler<CreateTenantCommand, Guid>
{
    public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = new Tenant
        {
            Name = request.Name
        };

        await tenantRepository.AddAsync(tenant);

        return tenant.Id;
    }
}