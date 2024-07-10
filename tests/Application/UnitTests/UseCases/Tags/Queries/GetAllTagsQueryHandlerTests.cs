using Application.UseCases.Tags.Queries;

namespace Application.UnitTests.UseCases.Tags.Queries;

[TestClass]
public class GetAllTagsQueryHandlerTests
{
    private readonly ILogger _logger = Substitute.For<ILogger>();
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly ITagRepository _tagRepository = Substitute.For<ITagRepository>();
    private readonly GetAllTagsQueryHandler _handler;

    public GetAllTagsQueryHandlerTests()
    {
        _handler = new GetAllTagsQueryHandler(_logger, _tenantRepository, _tagRepository);
    }

    [TestMethod]
    public async Task Handle_WithNonExistingTenantId_ThrowsNotFoundException()
    {
        var nonExistingTenantId = 999;
        var query = new GetAllTagsQuery(nonExistingTenantId, new BaseFilter());

        _tenantRepository.GetByIdAsync(nonExistingTenantId, default).Returns((Tenant)null!);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(query, default));

        Assert.AreEqual($"Tenant with ID {nonExistingTenantId} was not found.", exception.Message);
        await _tagRepository.DidNotReceiveWithAnyArgs().GetAllAsync(default, default!, default);
    }
}