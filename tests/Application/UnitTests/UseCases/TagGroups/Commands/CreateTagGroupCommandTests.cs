using Application.UseCases.TagGroups.Commands;

namespace UnitTests.UseCases.TagGroups.Commands;

[TestClass]
public class CreateTagGroupCommandTests
{
    private readonly ITagGroupRepository _tagGroupRepository = Substitute.For<ITagGroupRepository>();
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly CreateTagGroupCommandHandler _handler;

    public CreateTagGroupCommandTests()
    {
        _handler = new CreateTagGroupCommandHandler(_logger, _tagGroupRepository, _tenantRepository);

        _tenantRepository.GetByIdAsync(Arg.Any<int>(), default).Returns(new Tenant());
    }

    [TestMethod]
    public async Task CreateTagGroup_WithNonExistentTenant_ThrowsNotFoundException()
    {
        var command = new CreateTagGroupCommand("New TagGroup", 999); // 999 is a non-existent TenantId

        _tenantRepository.GetByIdAsync(command.TenantId, default).Returns((Tenant)null);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(command, default));

        Assert.AreEqual($"Tenant with ID {command.TenantId} was not found.", exception.Message);

        await _tagGroupRepository.DidNotReceiveWithAnyArgs().AddAsync(default!, default);
    }

    [TestMethod]
    public async Task CreateTagGroup_WithExistingName_ThrowsConflictException()
    {
        var existingTagGroup = new TagGroup
        {
            Name = "Existing TagGroup",
            TenantId = 1
        };

        _tagGroupRepository.GetTagGroupByNameAsync(existingTagGroup.TenantId, existingTagGroup.Name, default)
            .Returns(existingTagGroup);

        var command = new CreateTagGroupCommand(existingTagGroup.Name, existingTagGroup.TenantId);

        var exception = await Assert.ThrowsExceptionAsync<ConflictException>(() => _handler.Handle(command, default));

        Assert.AreEqual("Tag group with the same name for the tenant already exists", exception.Message);

        await _tagGroupRepository.DidNotReceiveWithAnyArgs().AddAsync(default!, default);
    }
}