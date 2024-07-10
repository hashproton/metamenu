using Application.UseCases.Tags.Commands;

namespace Application.IntegrationTests.UseCases.Tags.Commands;

[TestClass]
public class DeleteTagCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task Handle_DeleteTag_Success()
    {
        // Arrange: Create a tenant and tag group, and then a tag to delete
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
            Name = "Tag to Delete",
            TagGroupId = tagGroupId,
        };
        var tagId = await TagRepository.AddAsync(tag, default);

        var command = new DeleteTagCommand(tagId);

        // Act: Delete the tag
        await Mediator.Send(command);

        // Assert: Verify the tag was deleted successfully
        var deletedTag = await TagRepository.GetByIdAsync(tagId, default);
        Assert.IsNull(deletedTag);
    }
}