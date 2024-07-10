using Application.Errors;
using Application.UseCases.TagGroups.Queries;

namespace Application.UnitTests.UseCases.TagGroups.Queries;

[TestClass]
public class GetAllTagGroupsQueryTests
{
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly ITagGroupRepository _tagGroupRepository = Substitute.For<ITagGroupRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly GetAllTagGroupsQueryHandler _handler;

    public GetAllTagGroupsQueryTests()
    {
        _handler = new GetAllTagGroupsQueryHandler(_logger, _tenantRepository, _tagGroupRepository);
    }

    [TestMethod]
    public async Task GetAllTagGroups_WithNonExistingTenantId_ReturnsResult_NotFound()
    {
        var nonExistingTenantId = 999;
        var filter = new BaseFilter();

        _tenantRepository.GetByIdAsync(nonExistingTenantId, default).Returns((Tenant)null!);

        var query = new GetAllTagGroupsQuery(nonExistingTenantId, filter);

        var result = await _handler.Handle(query, default);
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(TenantErrors.TenantNotFound, result.Error);

        await _tagGroupRepository.DidNotReceive().GetAllAsync(Arg.Any<BaseFilter>(), Arg.Any<CancellationToken>());
    }
}