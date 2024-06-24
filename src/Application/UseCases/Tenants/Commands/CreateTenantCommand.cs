using Application.Services;

namespace Application.UseCases.Tenants.Commands;

public enum Role
{
    SuperAdmin,
    Admin,
    User
}

public class AuthContext
{
    public Guid UserId { get; set; }

    public List<int> TenantIds { get; set; } = [];

    public List<Role> Roles { get; set; } = [];
    
    public bool IsSuperAdmin => Roles.Contains(Role.SuperAdmin);
}

public class CreateTenantCommand(
    string name) : IRequest<Result<int>>
{
    public string Name { get; set; } = name;
}

public class CreateTenantCommandHandler(
    ILogger logger,
    ITenantRepository tenantRepository) : IRequestHandler<CreateTenantCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var existingTenant = await tenantRepository.GetTenantByNameAsync(request.Name, cancellationToken);
        if (existingTenant is not null)
        {
            return Result.Failure<int>(TenantErrors.TenantAlreadyExists);
        }

        var tenant = new Tenant
        {
            Name = request.Name,
            Status = TenantStatus.Inactive
        };

        await tenantRepository.AddAsync(tenant, cancellationToken);

        logger.LogInformation($"Tenant {tenant.Id} created");

        return Result.Success(tenant.Id);
    }
}