using Application.Errors;
using Application.UseCases.TagGroups.Commands;

namespace Application.UnitTests.UseCases.TagGroups.Commands;

[TestClass]
public class UpdateTagGroupCommandTests
{
    private readonly ITagGroupRepository _tagGroupRepository = Substitute.For<ITagGroupRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly UpdateTagGroupCommandHandler _handler;

    public UpdateTagGroupCommandTests()
    {
        _handler = new UpdateTagGroupCommandHandler(_logger, _tagGroupRepository);
    }

    [TestMethod]
    public async Task UpdateTagGroup_WithNonExistingId_ReturnsResult_NotFound()
    {
        var nonExistingTagGroupId = 1;

        _tagGroupRepository.GetByIdAsync(nonExistingTagGroupId, default).Returns((TagGroup)null!);

        var command = new UpdateTagGroupCommand(nonExistingTagGroupId)
        {
            Name = "New TagGroup Name"
        };

        var result = await _handler.Handle(command, default);
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(TagGroupErrors.TagGroupNotFound, result.Error);
        
        await _tagGroupRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }

    [TestMethod]
    public async Task UpdateTagGroup_WithExistingName_ReturnsResult_Conflict()
    {
        var existingTagGroup = new TagGroup
        {
            Id = 1,
            Name = "Existing TagGroup"
        };

        var anotherTagGroup = new TagGroup
        {
            Id = 2,
            Name = "Another TagGroup"
        };

        _tagGroupRepository.GetByIdAsync(anotherTagGroup.Id, default).Returns(anotherTagGroup);
        _tagGroupRepository.GetTagGroupByNameAsync(Arg.Any<int>(), existingTagGroup.Name, default)
            .Returns(existingTagGroup);

        var command = new UpdateTagGroupCommand(anotherTagGroup.Id)
        {
            Name = existingTagGroup.Name
        };

        var result = await _handler.Handle(command, default);
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(TagGroupErrors.TagGroupAlreadyExists, result.Error);
        
        await _tagGroupRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }
}