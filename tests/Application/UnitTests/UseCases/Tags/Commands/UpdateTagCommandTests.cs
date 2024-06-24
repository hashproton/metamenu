using Application.UseCases.Tags.Commands;

namespace Application.UnitTests.UseCases.Tags.Commands;

[TestClass]
public class UpdateTagCommandTests
{
    private readonly ITagGroupRepository _tagGroupRepository = Substitute.For<ITagGroupRepository>();
    private readonly ITagRepository _tagRepository = Substitute.For<ITagRepository>();
    private readonly UpdateTagCommandHandler _handler;

    public UpdateTagCommandTests()
    {
        _handler = new UpdateTagCommandHandler(_tagGroupRepository, _tagRepository);
    }

    [TestMethod]
    public async Task Handle_WithNonExistingTagId_ThrowsNotFoundException()
    {
        // Arrange
        var nonExistingTagId = 999;
        var command = new UpdateTagCommand(nonExistingTagId)
        {
            Name = "New Name"
        };

        _tagRepository.GetByIdAsync(nonExistingTagId, default).Returns((Tag)null!);

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(command, default));

        Assert.AreEqual($"Tag with ID {nonExistingTagId} was not found.", exception.Message);
        await _tagRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default!);
    }

    [TestMethod]
    public async Task Handle_WithNonExistingTagGroupId_ThrowsNotFoundException()
    {
        // Arrange
        var tagId = 1;
        var nonExistingTagGroupId = 999;
        var command = new UpdateTagCommand(tagId)
        {
            TagGroupId = nonExistingTagGroupId,
            Name = "New Name"
        };

        _tagRepository.GetByIdAsync(tagId, default).Returns(new Tag { Id = tagId });
        _tagGroupRepository.GetByIdAsync(nonExistingTagGroupId, default).Returns((TagGroup)null!);

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(command, default));

        Assert.AreEqual($"TagGroup with ID {nonExistingTagGroupId} was not found.", exception.Message);
        await _tagRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default!);
    }

    [TestMethod]
    public async Task Handle_WithExistingTagNameInTagGroup_ThrowsConflictException()
    {
        // Arrange
        var tagId = 1;
        var tagGroupId = 1;
        var existingTagName = "Existing Tag";
        var command = new UpdateTagCommand(tagId)
        {
            TagGroupId = tagGroupId,
            Name = existingTagName
        };

        var tag = new Tag { Id = tagId, TagGroupId = tagGroupId };
        var existingTag = new Tag
        {
            Id = 2, Name = existingTagName, TagGroupId = tagGroupId, TagGroup = new TagGroup { Name = "Test Group" }
        };

        _tagRepository.GetByIdAsync(tagId, default).Returns(tag);
        _tagGroupRepository.GetByIdAsync(tagGroupId, default).Returns(new TagGroup { Id = tagGroupId });
        _tagRepository.GetTagByNameAsync(tagGroupId, existingTagName, default).Returns(existingTag);

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<ConflictException>(() => _handler.Handle(command, default));

        Assert.AreEqual(
            $"A tag with the name '{existingTagName}' already exists in the tag group with Name 'Test Group'.",
            exception.Message);
        await _tagRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default!);
    }
}