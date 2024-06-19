using Application.UseCases.TagGroups.Commands;

namespace IntegrationTests.UseCases.TagGroups.Commands;

[TestClass]
public class DeleteTagGroupCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task DeleteTagGroup_Success()
    {
        // Arrange: Create a tag group to delete
        var tenantId = await TenantRepository.AddAsync(new()
        {
            Name = "Tenant to delete tag group"
        }, default);
        
        var tagGroupId = await TagGroupRepository.AddAsync(new()
        {
            Name = "TagGroup to delete",
            TenantId = tenantId
        }, default);

        // Act: Delete the tag group
        var deleteCommand = new DeleteTagGroupCommand(tagGroupId);
        await Mediator.Send(deleteCommand);

        // Assert: Verify the tag group was deleted
        var tenant = await TagGroupRepository.GetByIdAsync(tagGroupId, default);
        Assert.IsNull(tenant);
    }
}