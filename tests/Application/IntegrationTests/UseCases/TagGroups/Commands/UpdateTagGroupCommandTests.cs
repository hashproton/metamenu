using Application.UseCases.TagGroups.Commands;

namespace Application.IntegrationTests.UseCases.TagGroups.Commands;

[TestClass]
public class UpdateTagGroupCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task UpdateTagGroup_Success()
    {
        // Arrange: Create a tenant and a tag group to update
        var tenantId = await TenantRepository.AddAsync(new Tenant
            {
                Name = "Tenant for TagGroup"
            },
            default);

        var tagGroupId = await TagGroupRepository.AddAsync(new TagGroup
            {
                Name = "TagGroup to update",
                TenantId = tenantId
            },
            default);

        // Act: Update the tag group
        var updateCommand = new UpdateTagGroupCommand(tagGroupId)
        {
            Name = "Updated TagGroup"
        };

        await Mediator.Send(updateCommand);

        // Assert: Verify the tag group was updated
        var updatedTagGroup = await TagGroupRepository.GetByIdAsync(tagGroupId, default);
        Assert.IsNotNull(updatedTagGroup);
        Assert.AreEqual("Updated TagGroup", updatedTagGroup.Name);
        Assert.AreEqual(tagGroupId, updatedTagGroup.Id);
        Assert.AreEqual(tenantId, updatedTagGroup.TenantId);
    }
}