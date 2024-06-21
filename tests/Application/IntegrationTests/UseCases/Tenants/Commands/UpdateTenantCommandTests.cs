using Application.UseCases.Tenants.Commands;

namespace IntegrationTests.UseCases.Tenants.Commands;

[TestClass]
public class UpdateTenantCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task UpdateTenant_Success()
    {
        // Arrange: Create a tenant to update
        var tenantId = await TenantRepository.AddAsync(new Tenant
            {
                Name = "Tenant to update",
                Status = TenantStatus.Inactive
            },
            default);

        // Act: Update the tenant
        var updateCommand = new UpdateTenantCommand(tenantId)
        {
            Name = "Updated Tenant",
            Status = TenantStatus.Active
        };

        var result = await Mediator.Send(updateCommand);

        // Assert: Verify the tenant was updated
        Assert.IsTrue(result.IsSuccess);
        var tenant = await TenantRepository.GetByIdAsync(tenantId, default);
        Assert.IsNotNull(tenant);
        Assert.AreEqual("Updated Tenant", tenant.Name);
        Assert.AreEqual(TenantStatus.Active, tenant.Status);
    }
}