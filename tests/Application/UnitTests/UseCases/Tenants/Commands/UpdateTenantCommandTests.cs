using Application.Errors;
using Application.UseCases.Tenants.Commands;

namespace Application.UnitTests.UseCases.Tenants.Commands;

[TestClass]
public class UpdateTenantCommandTests
{
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly UpdateTenantCommandHandler _handler;

    public UpdateTenantCommandTests()
    {
        _handler = new UpdateTenantCommandHandler(_logger, _tenantRepository);
    }

    [TestMethod]
    public async Task UpdateTenant_WithNonExistingId_ReturnsResult_NotFound()
    {
        var nonExistingTenantId = 1;

        _tenantRepository.GetByIdAsync(nonExistingTenantId, default).Returns((Tenant)null!);

        var command = new UpdateTenantCommand(nonExistingTenantId)
        {
            Name = "New Tenant Name"
        };

        var result = await _handler.Handle(command, default);
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(TenantErrors.TenantNotFound, result.Error);

        await _tenantRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }

    [TestMethod]
    public async Task UpdateTenant_WithExistingName_ReturnsResult_Conflict()
    {
        var existingTenant = new Tenant
        {
            Id = 1,
            Name = "Existing Tenant"
        };

        var anotherTenant = new Tenant
        {
            Id = 2,
            Name = "Another Tenant"
        };

        _tenantRepository.GetByIdAsync(anotherTenant.Id, default).Returns(anotherTenant);
        _tenantRepository.GetTenantByNameAsync(existingTenant.Name, default).Returns(existingTenant);

        var command = new UpdateTenantCommand(anotherTenant.Id)
        {
            Name = existingTenant.Name
        };

        var result = await _handler.Handle(command, default);
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(TenantErrors.TenantAlreadyExists, result.Error);

        await _tenantRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }
}