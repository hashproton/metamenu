using Application.UseCases.TagGroups.Queries;

namespace UnitTests.UseCases.TagGroups.Queries;

[TestClass]
public class GetTagGroupByIdQueryTests
{
    private readonly ITagGroupRepository _tagGroupRepository = Substitute.For<ITagGroupRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly GetTagGroupByIdQueryHandler _handler;

    public GetTagGroupByIdQueryTests()
    {
        _handler = new GetTagGroupByIdQueryHandler(_logger, _tagGroupRepository);
    }

    [TestMethod]
    public async Task GetTagGroupById_WithNonExistingId_ThrowsNotFoundException()
    {
        var nonExistingTagGroupId = 1;

        _tagGroupRepository.GetByIdAsync(nonExistingTagGroupId, default).Returns((TagGroup)null!);

        var query = new GetTagGroupByIdQuery(nonExistingTagGroupId);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(query, default));

        Assert.AreEqual($"TagGroup with ID {nonExistingTagGroupId} was not found.", exception.Message);
    }
}