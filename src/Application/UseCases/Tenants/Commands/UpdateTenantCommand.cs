using Application.Services;

namespace Application.UseCases.Tenants.Commands;

public class UpdateTenantCommand(
    int id) : IRequest<Result>
{
    public int Id { get; set; } = id;

    public string? Name { get; set; }
}

public class UpdateTenantCommandHandler(
    ILogger logger,
    ITenantRepository tenantRepository) : IRequestHandler<UpdateTenantCommand, Result>
{
    public async Task<Result> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tenant is null)
        {
            return Result.Failure(TenantErrors.TenantNotFound);
        }

        var existingTenant = await tenantRepository.GetTenantByNameAsync(request.Name, cancellationToken);
        if (existingTenant is not null)
        {
            return Result.Failure(TenantErrors.TenantAlreadyExists);
        }

        if (request.Name is not null && request.Name != tenant.Name)
        {
            tenant.Name = request.Name;
        }

        await tenantRepository.UpdateAsync(tenant, cancellationToken);

        logger.LogInformation($"Tenant {tenant.Id} updated");
        
        return Result.Success();
    }
}