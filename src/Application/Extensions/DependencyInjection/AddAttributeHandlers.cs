using System.Reflection;
using Application.Attributes.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions.DependencyInjection;


public static partial class ServiceCollectionExtensions
{
    private static readonly ServiceLifetime DefaultLifetime = ServiceLifetime.Scoped;

    public static IServiceCollection AddAttributeHandlers(this IServiceCollection services)
    {
        var handlerInterfaceType = typeof(IAttributeHandler<>);

        var handlerTypes = typeof(Application.DependencyInjection).Assembly.GetTypes()
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            var lifetimeAttribute = handlerType.GetCustomAttribute<ServiceLifetimeAttribute>();
            var lifetime = lifetimeAttribute?.Lifetime ?? DefaultLifetime;

            var implementedInterfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType);

            foreach (var implementedInterface in implementedInterfaces)
            {
                services.Add(new(implementedInterface, handlerType, lifetime));
            }
        }

        return services;
    }
}
