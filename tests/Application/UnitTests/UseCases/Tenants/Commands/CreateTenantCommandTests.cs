using Application.Exceptions;
using Application.Repositories;
using Application.Services;
using Application.UseCases.Tenants.Commands;
using Domain.Entities;
using NSubstitute;

namespace UnitTests.UseCases.Tenants.Commands;

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
    public async Task CreateTenant_WithExistingName_ThrowsConflictException()
    {
        var existingTenant = new Tenant
        {
            Name = "Existing Tenant"
        };

        _tenantRepository.GetTenantByNameAsync(existingTenant.Name, default).Returns(existingTenant);

        var command = new CreateTenantCommand(existingTenant.Name);

        var exception = await Assert.ThrowsExceptionAsync<ConflictException>(() => _handler.Handle(command, default));

        Assert.AreEqual("Tenant with the same name already exists", exception.Message);

        await _tenantRepository.DidNotReceiveWithAnyArgs().AddAsync(default!, default);
    }
}