using System;

namespace Infrastructure.Logger.Factories
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger();
        ILogger CreateLogger(Guid id);
    }
}