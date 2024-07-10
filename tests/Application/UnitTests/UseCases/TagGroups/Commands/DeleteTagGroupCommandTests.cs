using Application.Errors;
using Application.UseCases.TagGroups.Commands;

namespace Application.UnitTests.UseCases.TagGroups.Commands;

[TestClass]
public class DeleteTagGroupCommandHandlerTests
{
    private readonly ITagGroupRepository _tagGroupRepository = Substitute.For<ITagGroupRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly DeleteTagGroupCommandHandler _handler;

    public DeleteTagGroupCommandHandlerTests()
    {
        _handler = new DeleteTagGroupCommandHandler(_logger, _tagGroupRepository);
    }

    [TestMethod]
    public async Task DeleteTagGroup_WithNonExistingId_ReturnsResult_NotFound()
    {
        var nonExistingTagGroupId = 1;

        _tagGroupRepository.GetByIdAsync(nonExistingTagGroupId, default).Returns((TagGroup)null!);

        var command = new DeleteTagGroupCommand(nonExistingTagGroupId);

        var result = await _handler.Handle(command, default);
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(TagGroupErrors.TagGroupNotFound, result.Error);

        await _tagGroupRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default!, default);
    }
}