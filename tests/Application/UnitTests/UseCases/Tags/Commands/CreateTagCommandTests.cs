using Application.UseCases.Tags.Commands;

namespace UnitTests.UseCases.Tags.Commands;

[TestClass]
public class CreateTagCommandTests
{
    private readonly ITagGroupRepository _tagGroupRepository = Substitute.For<ITagGroupRepository>();
    private readonly ITagRepository _tagRepository = Substitute.For<ITagRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly CreateTagCommandHandler _handler;

    public CreateTagCommandTests()
    {
        _handler = new CreateTagCommandHandler(_logger, _tagGroupRepository, _tagRepository);
    }

    [TestMethod]
    public async Task Handle_WithNonExistingTagGroupId_ThrowsNotFoundException()
    {
        var nonExistingTagGroupId = 999;
        var command = new CreateTagCommand(nonExistingTagGroupId, "New Tag");

        _tagGroupRepository.GetByIdAsync(nonExistingTagGroupId, default).Returns((TagGroup)null!);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(command, default));

        Assert.AreEqual($"TagGroup with ID {nonExistingTagGroupId} was not found.", exception.Message);
    }

    [TestMethod]
    public async Task Handle_WithExistingTagNameInTagGroup_ThrowsConflictException()
    {
        var tagGroupId = 1;
        var tagName = "Existing Tag";
        var command = new CreateTagCommand(tagGroupId, tagName);

        _tagGroupRepository.GetByIdAsync(tagGroupId, default).Returns(new TagGroup { Id = tagGroupId });
        _tagRepository.GetTagByNameAsync(tagGroupId, tagName, default).Returns(new Tag { Name = tagName });

        var exception = await Assert.ThrowsExceptionAsync<ConflictException>(() => _handler.Handle(command, default));

        Assert.AreEqual($"Tag with name {tagName} already exists in TagGroup {tagGroupId}", exception.Message);
    }
}