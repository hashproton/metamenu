using Application.Exceptions;
using Application.Repositories;
using Application.Services;
using Application.UseCases.Tenants.Commands;
using Domain.Entities;
using NSubstitute;

namespace UnitTests.UseCases.Tenants.Commands;

[TestClass]
public class UpdateTenantTests
{
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private readonly UpdateTenantCommandHandler _handler;

    public UpdateTenantTests()
    {
        _handler = new UpdateTenantCommandHandler(_logger, _tenantRepository);
    }

    [TestMethod]
    public async Task UpdateTenant_WithNonExistingId_ThrowsNotFoundException()
    {
        var nonExistingTenantId = 1;

        _tenantRepository.GetByIdAsync(nonExistingTenantId, default).Returns((Tenant)null);

        var command = new UpdateTenantCommand(nonExistingTenantId)
        {
            Name = "New Tenant Name"
        };

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(command, default));

        Assert.AreEqual($"Tenant with ID {nonExistingTenantId} was not found.", exception.Message);

        await _tenantRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }

    [TestMethod]
    public async Task UpdateTenant_WithExistingName_ThrowsConflictException()
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

        var exception = await Assert.ThrowsExceptionAsync<ConflictException>(() => _handler.Handle(command, default));

        Assert.AreEqual("Tenant with the same name already exists", exception.Message);

        await _tenantRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }
}