using Application.UseCases.Tags.Commands;

namespace IntegrationTests.UseCases.Tags.Commands;

[TestClass]
public class UpdateTagCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task Handle_UpdateTagName_Success()
    {
        // Arrange: Create a tenant and tag group, and then a tag to update
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
            Name = "Tag to Update",
            TagGroupId = tagGroupId,
        };
        var tagId = await TagRepository.AddAsync(tag, default);

        var updatedName = "Updated Tag Name";
        var command = new UpdateTagCommand(tagId)
        {
            Name = updatedName,
        };

        // Act: Update the tag name
        await Mediator.Send(command);

        // Assert: Verify the tag name was updated successfully
        var updatedTag = await TagRepository.GetByIdAsync(tagId, default);
        Assert.IsNotNull(updatedTag);
        Assert.AreEqual(updatedName, updatedTag.Name);
        Assert.AreEqual(tagGroupId, updatedTag.TagGroupId);
    }

    [TestMethod]
    public async Task Handle_UpdateTagGroup_Success()
    {
        // Arrange: Create a tenant and tag groups, and then a tag to update
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

        var newTagGroup = new TagGroup
        {
            Name = "New TagGroup",
            TenantId = tenantId,
        };
        var newTagGroupId = await TagGroupRepository.AddAsync(newTagGroup, default);

        var tag = new Tag
        {
            Name = "Tag to Update",
            TagGroupId = tagGroupId,
        };
        var tagId = await TagRepository.AddAsync(tag, default);

        var command = new UpdateTagCommand(tagId)
        {
            TagGroupId = newTagGroupId,
        };

        // Act: Update the tag group
        await Mediator.Send(command);

        // Assert: Verify the tag group was updated successfully
        var updatedTag = await TagRepository.GetByIdAsync(tagId, default);
        Assert.IsNotNull(updatedTag);
        Assert.AreEqual(newTagGroupId, updatedTag.TagGroupId);
        Assert.AreEqual("Tag to Update", updatedTag.Name);
    }

    [TestMethod]
    public async Task Handle_UpdateTagNameAndTagGroup_Success()
    {
        // Arrange: Create a tenant and tag groups, and then a tag to update
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

        var newTagGroup = new TagGroup
        {
            Name = "New TagGroup",
            TenantId = tenantId,
        };
        var newTagGroupId = await TagGroupRepository.AddAsync(newTagGroup, default);

        var tag = new Tag
        {
            Name = "Tag to Update",
            TagGroupId = tagGroupId,
        };
        var tagId = await TagRepository.AddAsync(tag, default);

        var updatedName = "Updated Tag Name";
        var command = new UpdateTagCommand(tagId)
        {
            Name = updatedName,
            TagGroupId = newTagGroupId,
        };

        // Act: Update the tag name and tag group
        await Mediator.Send(command);

        // Assert: Verify the tag name and tag group were updated successfully
        var updatedTag = await TagRepository.GetByIdAsync(tagId, default);
        Assert.IsNotNull(updatedTag);
        Assert.AreEqual(updatedName, updatedTag.Name);
        Assert.AreEqual(newTagGroupId, updatedTag.TagGroupId);
    }
}