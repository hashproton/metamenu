using Application.Services;

namespace Application.UseCases.Tenants.Commands;

public class CreateTenantCommand(
    string name) : IRequest<int>
{
    public string Name { get; set; } = name;
}

public class CreateTenantCommandHandler(
    ILogger logger,
    ITenantRepository tenantRepository) : IRequestHandler<CreateTenantCommand, int>
{
    public async Task<int> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var existingTenant = await tenantRepository.GetTenantByNameAsync(request.Name, cancellationToken);
        if (existingTenant is not null) throw new ConflictException("Tenant with the same name already exists");

        var tenant = new Tenant
        {
            Name = request.Name
        };

        await tenantRepository.AddAsync(tenant, cancellationToken);

        logger.LogInformation($"Tenant {tenant.Id} created");

        return tenant.Id;
    }
}