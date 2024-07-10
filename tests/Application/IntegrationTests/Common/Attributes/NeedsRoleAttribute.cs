using System.Reflection;
using Application.Models.Auth;

namespace Application.IntegrationTests.Common.Attributes;

public class NeedsRoleAttributeStrategy(AuthContext authContext) : ITestInitializationAttributeStrategy
{
    public void Initialize(MethodInfo testMethod)
    {
        var attribute = Attribute.GetCustomAttribute(testMethod, typeof(NeedsRoleAttribute));
        if (attribute is NeedsRoleAttribute needsRoleAttribute)
        {
            authContext.Roles.Add(needsRoleAttribute.Role);
        }
    }
}

internal sealed class NeedsRoleAttribute(Role role) : TestMethodAttribute
{
    public Role Role { get; } = role;
}