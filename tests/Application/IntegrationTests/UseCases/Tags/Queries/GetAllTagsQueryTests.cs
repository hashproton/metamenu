using Application.IntegrationTests.Common;
using Application.Repositories;
using Application.Repositories.Common;
using Application.UseCases.Tags.Queries;

namespace Application.IntegrationTests.UseCases.Tags.Queries;

[TestClass]
public class GetAllTagsQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task Handle_GetAllTags_Success()
    {
        // Arrange: Create a tenant, tag groups, and tags to retrieve
        var tenant = new Tenant
        {
            Name = "Tenant for TagGroup",
        };
        var tenantId = await TenantRepository.AddAsync(tenant, default);

        var tagGroup = new TagGroup
        {
            Name = "TagGroup for Tag",
            TenantId = tenantId,
        };
        var tagGroupId = await TagGroupRepository.AddAsync(tagGroup, default);

        var tags = new List<Tag>
        {
            new() { Name = "Tag1", TagGroupId = tagGroupId },
            new() { Name = "Tag2", TagGroupId = tagGroupId },
            new() { Name = "Tag3", TagGroupId = tagGroupId }
        };

        foreach (var tag in tags)
        {
            await TagRepository.AddAsync(tag, default);
        }

        var query = new GetAllTagsQuery(tenantId, new BaseFilter
        {
            PageNumber = 1,
            PageSize = 10
        });

        // Act: Retrieve the tags
        var result = await Mediator.Send(query);

        // Assert: Verify the tags were retrieved successfully
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Items.Count());
        Assert.AreEqual(1, result.PageNumber);
        Assert.AreEqual(10, result.PageSize);
        Assert.AreEqual(3, result.TotalItems);
        Assert.AreEqual(1, result.TotalPages);

        var firstTag = result.Items.First();
        Assert.AreEqual("Tag1", firstTag.Name);
        Assert.AreEqual(tagGroupId, firstTag.TagGroupId);
        Assert.AreEqual(tagGroup.Name, firstTag.TagGroupName);
    }
}
