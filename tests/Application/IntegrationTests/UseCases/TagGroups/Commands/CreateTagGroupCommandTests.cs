using Application.UseCases.TagGroups.Commands;

namespace IntegrationTests.UseCases.TagGroups.Commands;

[TestClass]
public class CreateTagGroupCommandTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task CreateTagGroup_Success()
    {
        // Arrange: Create a new tag group for an existing tenant
        var tenantId = await TenantRepository.AddAsync(new Tenant
            {
                Name = "Tenant for TagGroup"
            },
            default);

        var command = new CreateTagGroupCommand("New TagGroup", tenantId);

        // Act: Create the tag group
        var result = await Mediator.Send(command);

        // Assert: Verify the tag group was created
        Assert.IsNotNull(result);
        var tagGroup = await TagGroupRepository.GetByIdAsync(result, default);
        Assert.IsNotNull(tagGroup);
        Assert.AreEqual("New TagGroup", tagGroup.Name);
        Assert.AreEqual(tenantId, tagGroup.TenantId);
        Assert.AreEqual(result, tagGroup.Id);
    }
}