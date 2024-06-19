using Application.Exceptions;
using Application.Repositories;
using Application.Services;
using Application.UseCases.Tenants.Queries;
using Domain.Entities;
using NSubstitute;

namespace UnitTests.UseCases.Tenants.Queries;

[TestClass]
public class GetTenantByIdQueryTests
{
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly GetTenantByIdQueryHandler _handler;

    public GetTenantByIdQueryTests()
    {
        _handler = new GetTenantByIdQueryHandler(_logger, _tenantRepository);
    }

    [TestMethod]
    public async Task GetTenantById_WithNonExistingId_ThrowsNotFoundException()
    {
        var nonExistingTenantId = 1;

        _tenantRepository.GetByIdAsync(nonExistingTenantId, default).Returns((Tenant)null!);

        var query = new GetTenantByIdQuery(nonExistingTenantId);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(query, default));

        Assert.AreEqual($"Tenant with ID {nonExistingTenantId} was not found.", exception.Message);
    }
}