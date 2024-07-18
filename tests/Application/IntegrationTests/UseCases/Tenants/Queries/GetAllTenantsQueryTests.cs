using Application.Repositories;
using Application.UseCases.Tenants.Queries;

namespace Application.IntegrationTests.UseCases.Tenants.Queries;

[TestClass]
public class GetAllTenantsQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetAllTenants_PaginationPropertiesVerified_Success()
    {
        // Arrange: Create 15 tenants
        for (var i = 0; i < 15; i++)
        {
            await CreateTenantAsync(new() { Name = $"Tenant {i}" });
        }

        // Act: Retrieve all tenants with pagination
        var getAllTenantsQuery = new GetAllTenantsQuery(new TenantFilter
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

    [TestMethod]
    public async Task GetAllTenants_FilteredAndSorted_Success()
    {
        // Arrange: Create 10 tenants

        for (var i = 0; i < 10; i++)
        {
            await CreateTenantAsync(new() { Name = $"Tenant {i}" });
        }

        var expectedTenants = new List<Tenant>
        {
            new() { Name = "Random Tenant", Status = TenantStatus.Inactive },
            new() { Name = "Tenant 10", Status = TenantStatus.Demo },
            new() { Name = "Tenant 11", Status = TenantStatus.Demo },
        };

        foreach (var tenant in expectedTenants)
        {
            await TenantRepository.AddAsync(tenant, default);
        }

        // Act: Retrieve all tenants with pagination and filter
        // * filter: id must be greater than 10 and status must be Demo or Inactive
        var getAllTenantsQuery = new GetAllTenantsQuery(new TenantFilter
        {
            PageNumber = 1,
            PageSize = 5,
            FilterTerm = """ id > 10 && status ^^ ["Demo", "Inactive"] """,
            SortTerm = " id desc ",
        });

        var result = await Mediator.Send(getAllTenantsQuery);

        // Assert: Verify all tenants were retrieved and pagination properties
        Assert.IsTrue(result.IsSuccess);
        var paginatedTenants = result.Value;
        Assert.IsNotNull(paginatedTenants);
        Assert.AreEqual(3, paginatedTenants.Items.Count());
        Assert.AreEqual(1, paginatedTenants.PageNumber);
        Assert.AreEqual(5, paginatedTenants.PageSize);
        Assert.AreEqual(expectedTenants.Count, paginatedTenants.TotalItems);
        Assert.AreEqual(1, paginatedTenants.TotalPages);

        // Assert: Verify the tenants are sorted
        var actualTenants = paginatedTenants.Items.ToList();
        expectedTenants = expectedTenants.OrderByDescending(t => t.Id).ToList();

        for (var i = 0; i < expectedTenants.Count; i++)
        {
            Assert.AreEqual(expectedTenants[i].Name, actualTenants[i].Name);
            Assert.AreEqual(expectedTenants[i].Status, actualTenants[i].Status);
        }
    }
}