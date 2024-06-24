using Microsoft.Extensions.DependencyInjection;

namespace Application.Attributes.Common;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServiceLifetimeAttribute(ServiceLifetime lifetime) : Attribute
{
    public ServiceLifetime Lifetime { get; } = lifetime;
}