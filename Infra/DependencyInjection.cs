using Application.Repositories;
using Application.Repositories.Common;
using Infra.Repositories;
using Infra.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite("Data Source=app.db");
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<ITenantRepository, TenantRepository>();

        // is in development
        using var context = services.BuildServiceProvider().GetService<AppDbContext>()!;
        context.Database.Migrate();
    
        return services;
    }
}