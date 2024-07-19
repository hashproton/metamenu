using Application.Repositories.Common;
using Infra.Configuration;
using Infra.Identity.Configuration;
using Infra.Identity.Entities;
using Infra.Identity.Services;
using Infra.Repositories;
using Infra.Repositories.Common;
using Infra.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ILogger = Application.Services.ILogger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddSingleton<ILogger, Logger>();
        services.Configure<IdentityConfiguration>(configuration.GetSection(nameof(IdentityConfiguration)));

        services.AddDatabase(configuration);
        services.AddIdentity();

        return services;
    }

    private static void AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var databaseConfiguration = configuration
            .GetRequiredSection("Database")
            .Get<DatabaseConfiguration>()
            ?.Validate();

        services.AddDbContext<AppDbContext>(op => op.UseNpgsql(databaseConfiguration!.ConnectionString));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        var sp = services.BuildServiceProvider();
        var logger = sp.GetRequiredService<ILogger>();
        using var context = sp.GetRequiredService<AppDbContext>();

        var databaseExists = context.Database.CanConnect();
        if (!databaseExists)
        {
            try
            {
                logger.LogInformation("Database does not exist. Creating database...");
                context.Database.EnsureCreated();
            }
            catch (Exception e)
            {
                logger.LogError($"An error occurred while migrating the database ${e.Message}");
                throw;
            }
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

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();

        var identityConfiguration = sp.GetRequiredService<IOptions<IdentityConfiguration>>();

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Audience = identityConfiguration.Value.Audience;
                options.ClaimsIssuer = identityConfiguration.Value.Issuer;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        identityConfiguration.Value.SecretKey)),
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    },
                };
            });

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}