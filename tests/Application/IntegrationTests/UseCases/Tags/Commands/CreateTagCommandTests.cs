using Application.UseCases.Tags.Commands;

namespace Application.IntegrationTests.UseCases.Tags.Commands;

[TestClass]
public class CreateTagCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task Handle_CreateTag_Success()
    {
        // Arrange: Create a tenant and a tag group to associate with the tag
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

        var tagName = "New Tag";
        var command = new CreateTagCommand(tagGroupId, tagName);

        // Act: Create the tag
        var tagId = await Mediator.Send(command);

        // Assert: Verify the tag was created successfully
        var tag = await TagRepository.GetByIdAsync(tagId, default);
        Assert.IsNotNull(tag);
        Assert.AreEqual(tagName, tag.Name);
        Assert.AreEqual(tagGroupId, tag.TagGroupId);
        Assert.AreEqual(tenantId, tagGroup.TenantId);
    }
}