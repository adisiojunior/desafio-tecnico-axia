using Microsoft.Extensions.Logging;

namespace DesafioTecnicoAxia.Application.Common;

public abstract class BaseHandler
{
    protected readonly ILogger Logger;

    protected BaseHandler(ILogger logger)
    {
        Logger = logger;
    }

    protected void LogInformation(string message, params object[] args)
    {
        Logger.LogInformation(message, args);
    }

    protected void LogWarning(string message, params object[] args)
    {
        Logger.LogWarning(message, args);
    }

    protected void LogError(Exception exception, string message, params object[] args)
    {
        Logger.LogError(exception, message, args);
    }
}

