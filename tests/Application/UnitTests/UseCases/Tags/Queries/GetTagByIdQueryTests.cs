using Application.UseCases.Tags.Queries;

namespace Application.UnitTests.UseCases.Tags.Queries;

[TestClass]
public class GetTagByIdQueryTests
{
    private readonly ILogger _logger = Substitute.For<ILogger>();
    private readonly ITagRepository _tagRepository = Substitute.For<ITagRepository>();
    private readonly GetTagByIdQueryHandler _handler;

    public GetTagByIdQueryTests()
    {
        _handler = new GetTagByIdQueryHandler(_logger, _tagRepository);
    }

    [TestMethod]
    public async Task Handle_WithNonExistingTagId_ThrowsNotFoundException()
    {
        // Arrange
        var nonExistingTagId = 999;
        var query = new GetTagByIdQuery(nonExistingTagId);

        _tagRepository.GetByIdAsync(nonExistingTagId, default).Returns((Tag)null!);

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(query, default));

        Assert.AreEqual($"Tag with ID {nonExistingTagId} was not found.", exception.Message);
    }
}