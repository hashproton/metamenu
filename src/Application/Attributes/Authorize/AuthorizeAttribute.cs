using Application.UseCases.Tenants.Commands;

namespace Application.Attributes.Authorize;

[AttributeUsage(AttributeTargets.Class)]
public class AuthorizeAttribute(
    Role role) : Attribute
{
    public Role Role { get; set; } = role;
}