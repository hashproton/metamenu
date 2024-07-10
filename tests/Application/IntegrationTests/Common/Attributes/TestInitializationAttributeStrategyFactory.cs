using System.Reflection;
using Application.IntegrationTests.UseCases.Tenants.Queries;
using Application.Models.Auth;

namespace Application.IntegrationTests.Common.Attributes;

public static class TestInitializationAttributeStrategyFactory
{
    public static ITestInitializationAttributeStrategy? Create(MethodInfo testMethod, AuthContext authContext)
    {
        if (Attribute.GetCustomAttribute(testMethod, typeof(NeedsRoleAttribute)) is NeedsRoleAttribute)
        {
            return new NeedsRoleAttributeStrategy(authContext);
        }

        return null;
    }
}