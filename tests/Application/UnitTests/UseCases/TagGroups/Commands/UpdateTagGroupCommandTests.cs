using Application.Exceptions;
using Application.Repositories;
using Application.Services;
using Application.UseCases.TagGroups.Commands;
using Domain.Entities;
using NSubstitute;

namespace UnitTests.UseCases.TagGroups.Commands;

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
    public async Task UpdateTagGroup_WithNonExistingId_ThrowsNotFoundException()
    {
        var nonExistingTagGroupId = 1;

        _tagGroupRepository.GetByIdAsync(nonExistingTagGroupId, default).Returns((TagGroup)null!);

        var command = new UpdateTagGroupCommand(nonExistingTagGroupId)
        {
            Name = "New TagGroup Name"
        };

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(command, default));

        Assert.AreEqual($"TagGroup with ID {nonExistingTagGroupId} was not found.", exception.Message);

        await _tagGroupRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }

    [TestMethod]
    public async Task UpdateTagGroup_WithExistingName_ThrowsConflictException()
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
        _tagGroupRepository.GetTagGroupByNameAsync(Arg.Any<int>(), existingTagGroup.Name, default).Returns(existingTagGroup);

        var command = new UpdateTagGroupCommand(anotherTagGroup.Id)
        {
            Name = existingTagGroup.Name
        };

        var exception = await Assert.ThrowsExceptionAsync<ConflictException>(() => _handler.Handle(command, default));

        Assert.AreEqual("Tag Group with the same name for the tenant already exists", exception.Message);

        await _tagGroupRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }
}