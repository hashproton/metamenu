using Application.UseCases.TagGroups.Queries;

namespace Application.IntegrationTests.UseCases.TagGroups.Queries;

[TestClass]
public class GetTagGroupByIdQueryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetTagGroupById_Success()
    {
        // Arrange: Create a tenant and tag group to retrieve
        var tenantId = await TenantRepository.AddAsync(new Tenant
            {
                Name = "Tenant for TagGroup"
            },
            default);

        var tagGroupId = await TagGroupRepository.AddAsync(new TagGroup
            {
                Name = "TagGroup to retrieve",
                TenantId = tenantId
            },
            default);

        // Act: Retrieve the tag group
        var getTagGroupByIdQuery = new GetTagGroupByIdQuery(tagGroupId);
        var tagGroup = await Mediator.Send(getTagGroupByIdQuery);

        // Assert: Verify the tag group was retrieved
        Assert.IsNotNull(tagGroup);
        Assert.AreEqual("TagGroup to retrieve", tagGroup.Name);
        Assert.AreEqual(tagGroupId, tagGroup.Id);
        Assert.AreEqual(tenantId, tagGroup.TenantId);
    }
}