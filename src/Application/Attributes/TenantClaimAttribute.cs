using Microsoft.Extensions.DependencyInjection;

namespace Application.Attributes;

/// <summary>
/// Attribute to indicate that the request requires a tenant claim.
/// The tenant claim is assigned by the authentication service.
/// </summary>
public class TenantClaimAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServiceLifetimeAttribute(ServiceLifetime lifetime) : Attribute
{
    public ServiceLifetime Lifetime { get; } = lifetime;
}

