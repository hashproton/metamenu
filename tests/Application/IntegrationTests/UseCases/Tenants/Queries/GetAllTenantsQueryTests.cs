using Application.IntegrationTests.Common;
using Application.Repositories.Common;
using Application.UseCases.Tenants.Commands;
using Application.UseCases.Tenants.Queries;

namespace Application.IntegrationTests.UseCases.Tenants.Queries;

[TestClass]
public class GetAllTenantsQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetAllTenants_PaginationPropertiesVerified_Success()
    {
        AuthContext.Roles.Add(Role.SuperAdmin);
        // Arrange: Create tenants to retrieve
        for (var i = 0; i < 15; i++)
        {
            await CreateAuthedTenantAsync($"Tenant {i}");
        }

        // Act: Retrieve all tenants with pagination
        var getAllTenantsQuery = new GetAllTenantsQuery(new PaginatedQuery
        {
            PageNumber = 2,
            PageSize = 5
        });
        var result = await Mediator.Send(getAllTenantsQuery);

        // Assert: Verify all tenants were retrieved and pagination properties
        Assert.IsTrue(result.IsSuccess);
        var paginatedTenants = result.Value;
        Assert.IsNotNull(paginatedTenants);
        Assert.AreEqual(5, paginatedTenants.Items.Count());
        Assert.AreEqual(2, paginatedTenants.PageNumber);
        Assert.AreEqual(5, paginatedTenants.PageSize);
        Assert.AreEqual(15, paginatedTenants.TotalItems);
        Assert.AreEqual(3, paginatedTenants.TotalPages);
    }
}