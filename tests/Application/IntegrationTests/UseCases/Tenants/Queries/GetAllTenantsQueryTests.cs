using Application.Errors;
using Application.Exceptions;
using Application.Models.Auth;
using Application.Repositories;
using Application.UseCases.Tenants.Queries;

namespace Application.IntegrationTests.UseCases.Tenants.Queries;

[TestClass]
public class GetAllTenantsQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetAllTenants_PaginationPropertiesVerified_Success()
    {
        // Arrange:
        // * Set the current user as SuperAdmin
        // * Create 15 tenants
        AuthContext.Roles = [Role.SuperAdmin];

        for (var i = 0; i < 15; i++)
        {
            await CreateAuthedTenantAsync(new() { Name = $"Tenant {i}" });
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
        // Arrange:
        // * Set the current user as SuperAdmin
        // * Create 10 tenants
        AuthContext.Roles = [Role.SuperAdmin];

        for (var i = 0; i < 10; i++)
        {
            await CreateAuthedTenantAsync(new() { Name = $"Tenant {i}" });
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

    [TestMethod]
    public async Task GetAllTenants_Unauthorized_WhenUserIsNotSuperAdmin()
    {
        // Arrange: Set the current user as Admin
        AuthContext.Roles = [Role.Admin];

        // Act: Retrieve all tenants with pagination
        var getAllTenantsQuery = new GetAllTenantsQuery(new());
        var resultErrorException
            = await Assert.ThrowsExceptionAsync<ResultErrorException>(() => Mediator.Send(getAllTenantsQuery));

        // Assert: Verify the user is unauthorized
        Assert.AreEqual(AuthErrors.Forbidden, resultErrorException.Error);
    }
}