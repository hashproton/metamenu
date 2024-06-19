namespace Application.Models;

public enum EnvironmentKind
{
    Test,
    Development,
    Production
}

public static class EnvironmentExtensions
{
    public static EnvironmentKind ParseEnvironment(this string environment)
    {
        return environment switch
        {
            "Development" => EnvironmentKind.Development,
            "Production" => EnvironmentKind.Production,
            _ => throw new ArgumentException("Invalid environment")
        };
    }

    public static bool IsTest(this EnvironmentKind environmentKind)
    {
        return environmentKind == EnvironmentKind.Test;
    }

    public static bool IsDevelopment(this EnvironmentKind environmentKind)
    {
        return environmentKind == EnvironmentKind.Development;
    }

    public static bool IsProduction(this EnvironmentKind environmentKind)
    {
        return environmentKind == EnvironmentKind.Production;
    }
}