using Application.UseCases.Tenants.Commands;

namespace Application.IntegrationTests.UseCases.Tenants.Commands;

[TestClass]
public class DeleteTenantCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task DeleteTenant_Success()
    {
        // Arrange: Create a tenant to delete
        var tenantId = await TenantRepository.AddAsync(new Tenant
            {
                Name = "Tenant to delete"
            },
            default);

        // Act: Delete the tenant
        var deleteCommand = new DeleteTenantCommand(tenantId);
        var result = await Mediator.Send(deleteCommand);

        // Assert: Verify the tenant was deleted
        Assert.IsTrue(result.IsSuccess);
        var tenant = await TenantRepository.GetByIdAsync(tenantId, default);
        Assert.IsNull(tenant);
    }
}