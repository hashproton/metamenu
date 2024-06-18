using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Tenants.Commands;

public class CreateTenantCommand(
    string name) : IRequest<Guid>
{
    public string Name { get; set; } = name;
}

public class CreateTenantCommandHandler(ITenantRepository tenantRepository) : IRequestHandler<CreateTenantCommand, Guid>
{
    public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = new Tenant
        {
            Name = request.Name
        };

        await tenantRepository.AddAsync(tenant, cancellationToken);

        return tenant.Id;
    }
}