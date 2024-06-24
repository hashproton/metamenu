using Application.IntegrationTests.Common;
using Application.UseCases.TagGroups.Commands;

namespace Application.IntegrationTests.UseCases.TagGroups.Commands;

[TestClass]
public class DeleteTagGroupCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task DeleteTagGroup_Success()
    {
        // Arrange: Create a tag group to delete
        var tenantId = await TenantRepository.AddAsync(new Tenant
            {
                Name = "Tenant to delete tag group"
            },
            default);

        var tagGroupId = await TagGroupRepository.AddAsync(new TagGroup
            {
                Name = "TagGroup to delete",
                TenantId = tenantId
            },
            default);

        // Act: Delete the tag group
        var deleteCommand = new DeleteTagGroupCommand(tagGroupId);
        await Mediator.Send(deleteCommand);

        // Assert: Verify the tag group was deleted
        var tagGroup = await TagGroupRepository.GetByIdAsync(tagGroupId, default);
        Assert.IsNull(tagGroup);
    }
}