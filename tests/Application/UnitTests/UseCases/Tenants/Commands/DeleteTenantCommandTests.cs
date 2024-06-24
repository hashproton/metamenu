using Application.Errors;
using Application.UseCases.Tenants.Commands;

namespace Application.UnitTests.UseCases.Tenants.Commands;

[TestClass]
public class DeleteTenantCommandTests
{
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly DeleteTenantCommandHandler _handler;

    public DeleteTenantCommandTests()
    {
        _handler = new DeleteTenantCommandHandler(_logger, _tenantRepository);
    }

    [TestMethod]
    public async Task DeleteTenant_WithNonExistingId_ThrowsNotFoundException()
    {
        var nonExistingTenantId = 1;

        _tenantRepository.GetByIdAsync(nonExistingTenantId, default).Returns((Tenant)null!);

        var command = new DeleteTenantCommand(nonExistingTenantId);

        var result = await _handler.Handle(command, default);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(TenantErrors.TenantNotFound.Message, result.Error!.Message);

        await _tenantRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default!, default);
    }
}