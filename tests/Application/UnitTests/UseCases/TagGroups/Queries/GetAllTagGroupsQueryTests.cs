using Application.Exceptions;
using Application.Repositories;
using Application.Repositories.Common;
using Application.Services;
using Application.UseCases.TagGroups.Queries;
using Domain.Entities;
using NSubstitute;

namespace UnitTests.UseCases.TagGroups.Queries;

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
    public async Task GetAllTagGroups_WithNonExistingTenantId_ThrowsNotFoundException()
    {
        var nonExistingTenantId = 999;
        var paginatedQuery = new PaginatedQuery(1, 10);

        _tenantRepository.GetByIdAsync(nonExistingTenantId, default).Returns((Tenant)null!);

        var query = new GetAllTagGroupsQuery(nonExistingTenantId, paginatedQuery);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(query, default));

        Assert.AreEqual($"Tenant with ID {nonExistingTenantId} was not found.", exception.Message);

        await _tagGroupRepository.DidNotReceive().GetAllAsync(Arg.Any<PaginatedQuery>(), Arg.Any<CancellationToken>());
    }
}