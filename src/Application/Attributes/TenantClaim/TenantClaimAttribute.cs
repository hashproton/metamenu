namespace Application.Attributes.TenantClaim;

/// <summary>
/// Attribute to indicate that the request requires a tenant claim.
/// The tenant claim is assigned by the authentication service.
/// </summary>
public class TenantClaimAttribute : Attribute
{
}