using Application.Repositories;
using Application.UseCases.Tenants.Commands;
using Domain.Entities;
using NSubstitute;

namespace UnitTests.UseCases.Tenants.Commands;

[TestClass]
public class CreateTenantTests
{
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly CreateTenantCommandHandler _handler;
    
    public CreateTenantTests()
    {
        _handler = new CreateTenantCommandHandler(_tenantRepository);
    }
    
    [TestMethod]
    public async Task CreateTenant_Success()
    {
        var command = new CreateTenantCommand("Tenant Name");
        
        var result = await _handler.Handle(command, default);        
        
        Assert.IsNotNull(result);
        await _tenantRepository.Received(1).AddAsync(Arg.Any<Tenant>(), default);
    }
}