using Application.UseCases.Tenants.Commands;

namespace UnitTests.UseCases.Tenants.Commands;

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

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(command, default));

        Assert.AreEqual($"Tenant with ID {nonExistingTenantId} was not found.", exception.Message);

        await _tenantRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default!, default);
    }
}