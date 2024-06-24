using Application.UseCases.Tenants.Commands;

namespace Application.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AuthorizeAttribute(
    Role role) : Attribute
{
    public Role Role { get; set; } = role;
}