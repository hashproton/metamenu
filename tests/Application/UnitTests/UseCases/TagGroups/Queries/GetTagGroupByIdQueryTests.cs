using Application.UseCases.TagGroups.Queries;

namespace Application.UnitTests.UseCases.TagGroups.Queries;

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
    public async Task GetTagGroupById_WithNonExistingId_ReturnsResult_NotFound()
    {
        var nonExistingTagGroupId = 1;

        _tagGroupRepository.GetByIdAsync(nonExistingTagGroupId, default).Returns((TagGroup)null!);

        var query = new GetTagGroupByIdQuery(nonExistingTagGroupId);

        var result = await _handler.Handle(query, default);
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Errors);
    }
}