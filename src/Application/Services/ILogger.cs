namespace Application.Services;

public interface ILogger
{
    void LogInformation(string message);
    
    void LogError(string message);
}