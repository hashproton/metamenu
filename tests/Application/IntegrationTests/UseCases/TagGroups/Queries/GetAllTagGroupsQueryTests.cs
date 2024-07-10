using Application.UseCases.TagGroups.Queries;

namespace Application.IntegrationTests.UseCases.TagGroups.Queries;

[TestClass]
public class GetAllTagGroupsQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetAllTagGroups_PaginationPropertiesVerified_Success()
    {
        // Arrange: Create a tenant and tag groups to retrieve
        var tenantId = await TenantRepository.AddAsync(new Tenant
            {
                Name = "Tenant for TagGroups"
            },
            default);

        for (var i = 0; i < 15; i++)
        {
            await TagGroupRepository.AddAsync(new TagGroup
                {
                    Name = $"TagGroup {i + 1}",
                    TenantId = tenantId
                },
                default);
        }

        // Act: Retrieve all tag groups with pagination
        var getAllTagGroupsQuery = new GetAllTagGroupsQuery(tenantId, new BaseFilter
        {
            PageNumber = 2,
            PageSize = 5
        });
        var result = await Mediator.Send(getAllTagGroupsQuery);

        // Assert: Verify all tag groups were retrieved and pagination properties
        var paginatedTagGroups = result.Value;

        Assert.IsNotNull(paginatedTagGroups);
        Assert.AreEqual(5, paginatedTagGroups.Items.Count());
        Assert.AreEqual(2, paginatedTagGroups.PageNumber);
        Assert.AreEqual(5, paginatedTagGroups.PageSize);
        Assert.AreEqual(15, paginatedTagGroups.TotalItems);
        Assert.AreEqual(3, paginatedTagGroups.TotalPages);
    }
}