using Application.Services;

namespace Application.UseCases.Tenants.Commands;

public class UpdateTenantCommand(
    int id) : IRequest<Result>
{
    public int Id { get; set; } = id;

    public string? Name { get; set; }
    
    public TenantStatus? Status { get; set; }
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

        if (request.Name is not null && request.Name != tenant.Name)
        {
            var existingTenant = await tenantRepository.GetTenantByNameAsync(request.Name, cancellationToken);
            if (existingTenant is not null)
            {
                return Result.Failure(TenantErrors.TenantAlreadyExists);
            }

            tenant.Name = request.Name;
        }
        
        if (request.Status is not null && request.Status != tenant.Status)
        {
            logger.LogInformation($"Updating tenant {tenant.Id} status to {request.Status.Value}");
            tenant.Status = request.Status.Value;
        }

        await tenantRepository.UpdateAsync(tenant, cancellationToken);

        logger.LogInformation($"Tenant {tenant.Id} updated");
        
        return Result.Success();
    }
}