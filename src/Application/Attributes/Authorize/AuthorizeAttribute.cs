using Application.Models.Auth;

namespace Application.Attributes.Authorize;

[AttributeUsage(AttributeTargets.Class)]
public class AuthorizeAttribute(
    Role role) : Attribute
{
    public Role Role { get; set; } = role;
}