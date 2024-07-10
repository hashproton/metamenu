using Application.UseCases.Tags.Queries;

namespace Application.IntegrationTests.UseCases.Tags.Queries;

[TestClass]
public class GetTagByIdQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task Handle_GetTagById_Success()
    {
        // Arrange: Create a tenant and tag group, and then a tag to retrieve
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

        var tag = new Tag
        {
            Name = "Tag to Retrieve",
            TagGroupId = tagGroupId,
        };
        var tagId = await TagRepository.AddAsync(tag, default);

        var query = new GetTagByIdQuery(tagId);

        // Act: Retrieve the tag
        var result = await Mediator.Send(query);

        // Assert: Verify the tag was retrieved successfully
        Assert.IsNotNull(result);
        Assert.AreEqual(tagId, result.Id);
        Assert.AreEqual(tag.Name, result.Name);
        Assert.AreEqual(tagGroupId, result.TagGroupId);
        Assert.AreEqual(tagGroup.Name, result.TagGroupName);
    }
}
