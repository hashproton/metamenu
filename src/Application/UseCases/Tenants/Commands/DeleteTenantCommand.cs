using Application.Services;

namespace Application.UseCases.Tenants.Commands;

public class DeleteTenantCommand(
    int id) : IRequest<Result>
{
    public int Id { get; set; } = id;
}

public class DeleteTenantCommandHandler(
    ILogger logger,
    ITenantRepository tenantRepository) : IRequestHandler<DeleteTenantCommand, Result>
{
    public async Task<Result> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tenant is null)
        {
            return Result.Failure(TenantErrors.TenantNotFound);
        }

        await tenantRepository.DeleteAsync(tenant, cancellationToken);

        logger.LogInformation($"Tenant {tenant.Id} deleted");
        
        return Result.Success();
    }
}