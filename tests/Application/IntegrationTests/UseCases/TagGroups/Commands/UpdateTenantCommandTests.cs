using Application.UseCases.TagGroups.Commands;

namespace IntegrationTests.UseCases.TagGroups.Commands;

[TestClass]
public class UpdateTagGroupCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task UpdateTagGroup_Success()
    {
        // Arrange: Create a tag group to update
        var tenantId = await TenantRepository.AddAsync(new()
        {
            Name = "Tenant for TagGroup"
        }, default);
        
        var tagGroupId = await TagGroupRepository.AddAsync(new()
        {
            Name = "TagGroup to update",
            TenantId = tenantId
        }, default);

        // Act: Update the tag group
        var updateCommand = new UpdateTagGroupCommand(tagGroupId)
        {
            Name = "Updated TagGroup",
        };

        await Mediator.Send(updateCommand);

        // Assert: Verify the tag group was updated
        var tagGroup = await TagGroupRepository.GetByIdAsync(tagGroupId, default);
        Assert.IsNotNull(tagGroup);
        Assert.AreEqual("Updated TagGroup", tagGroup.Name);
    }
}