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
    public async Task Handle_InvalidTagGroup_ThrowsNotFoundException()
    {
        var command = new DeleteTagGroupCommand(1);
        _tagGroupRepository.GetByIdAsync(command.Id, default).Returns((TagGroup)null!);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(command, default));
        Assert.AreEqual($"TagGroup with ID {command.Id} was not found.", exception.Message);
        await _tagGroupRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default!, default);
    }
}