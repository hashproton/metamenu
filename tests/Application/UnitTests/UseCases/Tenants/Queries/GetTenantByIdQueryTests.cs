using Application.Errors;
using Application.UseCases.Tenants.Queries;

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

        var result = await _handler.Handle(query, default);
        
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(TenantErrors.TenantNotFound.Message, result.Error!.Message);

        await _tenantRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default!, default);
    }
}