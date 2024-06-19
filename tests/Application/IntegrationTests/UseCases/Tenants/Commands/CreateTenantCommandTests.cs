using Application.UseCases.Tenants.Commands;

namespace IntegrationTests.UseCases.Tenants.Commands;

[TestClass]
public class CreateTenantCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task CreateTenant_Success()
    {
        // Arrange: Create a new tenant
        var command = new CreateTenantCommand("New Tenant");

        // Act: Create the tenant
        var result = await Mediator.Send(command);

        // Assert: Verify the tenant was created
        Assert.IsNotNull(result);
        var tenant = await TenantRepository.GetByIdAsync(result, default);
        Assert.IsNotNull(tenant);
        Assert.AreEqual("New Tenant", tenant.Name);
        Assert.AreEqual(result, tenant.Id);
    }
}