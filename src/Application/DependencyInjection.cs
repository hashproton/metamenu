using Application.Attributes.Common;
using Application.Extensions;
using Application.Extensions.DependencyInjection;
using Application.UseCases.Tenants.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenRequestPreProcessor(typeof(AttributeHandlerPreProcessor<>));
        });

        services.AddAttributeHandlers();

        services.AddScoped<AuthContext>();

        return services;
    }
}