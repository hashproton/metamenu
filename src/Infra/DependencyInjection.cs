using Application.Models;
using Application.Repositories.Common;
using Application.Services.AuthService;
using Infra.Configuration;
using Infra.Repositories;
using Infra.Repositories.Common;
using Infra.Services;
using Infra.Services.AuthService;
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
        services.AddSingleton<IAuthService, AuthService>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddDatabase(configuration, environment);

        services.AddHttpClient("identipass",
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:5124");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

        return services;
    }

    private static void AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration,
        EnvironmentKind environment)
    {
        var databaseConfiguration = configuration
            .GetRequiredSection("Database")
            .Get<DatabaseConfiguration>()
            ?.Validate();

        services.AddDbContext<AppDbContext>(op => op.UseNpgsql(databaseConfiguration!.ConnectionString));

        var sp = services.BuildServiceProvider();
        var logger = sp.GetRequiredService<ILogger>();
        using var context = sp.GetRequiredService<AppDbContext>();

        var databaseExists = context.Database.CanConnect();
        if (!databaseExists)
        {
            logger.LogInformation("Database does not exist. Creating database...");

            context.Database.EnsureCreated();
        }

        try
        {
            if (databaseExists)
            {
                context.Database.Migrate();
            }
        }
        catch (Exception e)
        {
            logger.LogError($"An error occurred while migrating the database ${e.Message}");
            throw;
        }

        var repositoriesInterfaces = typeof(IGenericRepository<>).Assembly.GetTypes()
            .Where(t => t.IsInterface && t.Name.EndsWith("Repository"))
            .ToList();

        var repositoriesImplementations = typeof(GenericRepository<>).Assembly.GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Repository"))
            .ToList();

        foreach (var repositoryInterface in repositoriesInterfaces)
        {
            var repositoryImplementation = repositoriesImplementations
                .FirstOrDefault(t => t.Name == repositoryInterface.Name[1..]);

            if (repositoryImplementation is null)
            {
                throw new InvalidOperationException(
                    $"Repository implementation for {repositoryInterface.Name} not found");
            }

            services.AddScoped(repositoryInterface, repositoryImplementation);
        }
    }
}