using Application.UseCases.Tags.Commands;

namespace Application.UnitTests.UseCases.Tags.Commands;

[TestClass]
public class DeleteTagCommandTests
{
    private readonly ILogger _logger = Substitute.For<ILogger>();
    private readonly ITagRepository _tagRepository = Substitute.For<ITagRepository>();
    private readonly DeleteTagCommandHandler _handler;

    public DeleteTagCommandTests()
    {
        _handler = new DeleteTagCommandHandler(_logger, _tagRepository);
    }

    [TestMethod]
    public async Task Handle_WithNonExistingTagId_ThrowsNotFoundException()
    {
        var nonExistingTagId = 999;
        var command = new DeleteTagCommand(nonExistingTagId);

        _tagRepository.GetByIdAsync(nonExistingTagId, default).Returns((Tag)null!);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(command, default));

        Assert.AreEqual($"Tag with ID {nonExistingTagId} was not found.", exception.Message);

        await _tagRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default!, default!);
    }
}