using System.Reflection;

namespace Application.IntegrationTests.Common.Attributes;

public interface ITestInitializationAttributeStrategy
{
    void Initialize(MethodInfo testMethod);
}