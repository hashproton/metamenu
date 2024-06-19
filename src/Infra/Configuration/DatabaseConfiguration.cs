namespace Infra.Configuration;

public class DatabaseConfiguration
{
    public string ConnectionString { get; set; } = default!;

    public DatabaseConfiguration Validate()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new ArgumentException("Database connection string is required");
        }

        return this;
    }
}