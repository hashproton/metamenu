using Application;
using Application.Models;
using Application.Repositories;
using Infra;
using Infra.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Common;

[TestClass]
public class BaseIntegrationTest
{
    private static ServiceProvider _provider = null!;

    protected ITenantRepository TenantRepository => _provider.GetRequiredService<ITenantRepository>();

    protected ITagGroupRepository TagGroupRepository => _provider.GetRequiredService<ITagGroupRepository>();

    protected ISender Mediator => _provider.GetRequiredService<ISender>();

    private static AppDbContext DbContext => _provider.GetRequiredService<AppDbContext>();

    [AssemblyInitialize]
    public static async Task AssemblyInitialize(TestContext context)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection()
            .AddApplication()
            .AddInfra(EnvironmentKind.Development, configuration);

        _provider = services.BuildServiceProvider();
        await DbContext.Database.MigrateAsync();
    }

    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        await DbContext.Database.EnsureDeletedAsync();
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        await DbContext.Tenants.ExecuteDeleteAsync();
    }
}