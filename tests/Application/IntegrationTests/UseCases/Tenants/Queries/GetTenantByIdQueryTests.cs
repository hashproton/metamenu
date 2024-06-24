using Application.IntegrationTests.Common;
using Application.UseCases.Tenants.Queries;

namespace Application.IntegrationTests.UseCases.Tenants.Queries;

[TestClass]
public class GetTenantByIdQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetTenantById_Success()
    {
        // Arrange: Create a tenant to retrieve
        var tenantId = await CreateAuthedTenantAsync("Tenant to retrieve");

        // Act: Retrieve the tenant
        var getTenantByIdQuery = new GetTenantByIdQuery(tenantId);
        var result = await Mediator.Send(getTenantByIdQuery);

        // Assert: Verify the tenant was retrieved
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Tenant to retrieve", result.Value.Name);
        Assert.AreEqual(tenantId, result.Value.Id);
    }
}