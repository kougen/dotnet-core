using System;

namespace Infrastructure.Logger.Factories
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Guid id);
    }
}