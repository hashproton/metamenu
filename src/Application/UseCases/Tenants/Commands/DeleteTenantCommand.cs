using Application.Services;

namespace Application.UseCases.Tenants.Commands;

public class DeleteTenantCommand(
    int id) : IRequest
{
    public int Id { get; set; } = id;
}

public class DeleteTenantCommandHandler(
    ILogger logger,
    ITenantRepository tenantRepository) : IRequestHandler<DeleteTenantCommand>
{
    public async Task Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tenant is null)
        {
            throw new NotFoundException(nameof(Tenant), request.Id);
        }

        await tenantRepository.DeleteAsync(tenant, cancellationToken);

        logger.LogInformation($"Tenant {tenant.Id} deleted");
    }
}