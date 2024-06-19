using Application.Models;
using Application.Repositories;
using Application.Repositories.Common;
using Infra.Configuration;
using Infra.Repositories;
using Infra.Repositories.Common;
using Infra.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ILogger = Application.Services.ILogger;

namespace Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(
        this IServiceCollection services,
        EnvironmentKind environment,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddSingleton<ILogger, Logger>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITagGroupRepository, TagGroupRepository>();
        services.AddDatabase(configuration);

        if (environment.IsDevelopment())
        {
            using var context = services.BuildServiceProvider().GetService<AppDbContext>()!;

            context.Database.Migrate();
        }

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration = configuration
            .GetRequiredSection("Database")
            .Get<DatabaseConfiguration>()
            ?.Validate();

        services.AddDbContext<AppDbContext>(op => op.UseNpgsql(databaseConfiguration!.ConnectionString));
    }
}