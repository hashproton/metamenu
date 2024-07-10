using Application.Errors;
using Application.UseCases.Tenants.Commands;

namespace Application.UnitTests.UseCases.Tenants.Commands;

[TestClass]
public class CreateTenantCommandTests
{
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly CreateTenantCommandHandler _handler;

    public CreateTenantCommandTests()
    {
        _handler = new CreateTenantCommandHandler(_logger, _tenantRepository);
    }

    [TestMethod]
    public async Task CreateTenant_WithExistingName_ReturnsResult_Conflict()
    {
        var existingTenant = new Tenant
        {
            Name = "Existing Tenant"
        };

        _tenantRepository.GetTenantByNameAsync(existingTenant.Name, default).Returns(existingTenant);

        var command = new CreateTenantCommand(existingTenant.Name);

        var result = await _handler.Handle(command, default);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(TenantErrors.TenantAlreadyExists, result.Error);

        await _tenantRepository.DidNotReceiveWithAnyArgs().AddAsync(default!, default);
    }
}