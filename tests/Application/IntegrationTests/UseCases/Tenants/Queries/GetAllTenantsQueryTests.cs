using Application.Repositories.Common;
using Application.UseCases.Tenants.Queries;

namespace IntegrationTests.UseCases.Tenants.Queries;

[TestClass]
public class GetAllTenantsQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetAllTenants_PaginationPropertiesVerified_Success()
    {
        // Arrange: Create tenants to retrieve
        for (var i = 0; i < 15; i++)
        {
            await TenantRepository.AddAsync(new Tenant
                {
                    Name = $"Tenant {i + 1}"
                },
                default);
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