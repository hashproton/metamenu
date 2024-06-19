using Application.UseCases.Tenants.Queries;

namespace IntegrationTests.UseCases.Tenants.Queries;

[TestClass]
public class GetTenantByIdQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetTenantById_Success()
    {
        // Arrange: Create a tenant to retrieve
        var tenantId = await TenantRepository.AddAsync(new()
        {
            Name = "Tenant to retrieve"
        }, default);

        // Act: Retrieve the tenant
        var getTenantByIdQuery = new GetTenantByIdQuery(tenantId);
        var tenant = await Mediator.Send(getTenantByIdQuery);

        // Assert: Verify the tenant was retrieved
        Assert.IsNotNull(tenant);
        Assert.AreEqual("Tenant to retrieve", tenant.Name);
        Assert.AreEqual(tenantId, tenant.Id);
    }
}