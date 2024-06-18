using Application.Repositories;
using Application.Repositories.Common;
using Application.UseCases.Tenants.Queries;
using Domain.Entities;
using NSubstitute;

namespace UnitTests.UseCases.Tenants.Queries;

[TestClass]
public class GetAllTenantsQueryTests
{
    private readonly List<Tenant> _tenants = [
        new() { Id = Guid.NewGuid(), Name = "Tenant 1" },
        new() { Id = Guid.NewGuid(), Name = "Tenant 2" },
    ];
    
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    
    private readonly GetAllTenantsQueryHandler _handler;
    
    public GetAllTenantsQueryTests()
    {
        _tenantRepository.GetAllAsync(Arg.Any<PaginatedQuery>(), default).Returns(new PaginatedResult<Tenant>(_tenants, _tenants.Count, 1, 10, 10));
        
        _handler = new GetAllTenantsQueryHandler(_tenantRepository);
    }
    
    [TestMethod]
    public async Task GetAllTenants_Success()
    {
        var query = new GetAllTenantsQuery(new PaginatedQuery(1, 10));
        
        var result = await _handler.Handle(query, default);
        
        Assert.IsNotNull(result);
        await _tenantRepository.Received().GetAllAsync(Arg.Any<PaginatedQuery>(), default);
        Assert.AreEqual(_tenants.Count, result.Items.Count());
        foreach (var tenant in _tenants)
        {
            var tenantDto = result.Items.FirstOrDefault(t => t.Id == tenant.Id);
            
            Assert.IsNotNull(tenantDto);
            Assert.AreEqual(tenant.Name, tenantDto.Name);
        }
    }
}