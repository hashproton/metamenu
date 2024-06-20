using Application.UseCases.Tenants.Queries;

namespace IntegrationTests.UseCases.Tenants.Queries;

[TestClass]
public class GetTenantByIdQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetTenantById_Success()
    {
        // Arrange: Create a tenant to retrieve
        var tenantId = await TenantRepository.AddAsync(new Tenant
            {
                Name = "Tenant to retrieve"
            },
            default);

        // Act: Retrieve the tenant
        var getTenantByIdQuery = new GetTenantByIdQuery(tenantId);
        var result = await Mediator.Send(getTenantByIdQuery);

        // Assert: Verify the tenant was retrieved
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("Tenant to retrieve", result.Value.Name);
        Assert.AreEqual(tenantId, result.Value.Id);
    }
}