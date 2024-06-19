using Serilog;
using ILogger = Application.Services.ILogger;

namespace Infra.Services;

public class Logger : ILogger
{
    public void LogInformation(string message)
    {
        Log.Information(message);
    }
}