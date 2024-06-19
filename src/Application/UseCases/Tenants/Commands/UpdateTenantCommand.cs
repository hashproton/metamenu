using Application.Services;

namespace Application.UseCases.Tenants.Commands;

public class UpdateTenantCommand(
    int id) : IRequest
{
    public int Id { get; set; } = id;

    public string? Name { get; set; }
}

public class UpdateTenantCommandHandler(
    ILogger logger,
    ITenantRepository tenantRepository) : IRequestHandler<UpdateTenantCommand>
{
    public async Task Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tenant is null) throw new NotFoundException(nameof(Tenant), request.Id);

        var existingTenant = await tenantRepository.GetTenantByNameAsync(request.Name, cancellationToken);
        if (existingTenant is not null) throw new ConflictException("Tenant with the same name already exists");

        if (request.Name is not null && request.Name != tenant.Name) tenant.Name = request.Name;

        await tenantRepository.UpdateAsync(tenant, cancellationToken);

        logger.LogInformation($"Tenant {tenant.Id} updated");
    }
}