using System;
using Infrastructure.Logger;
using Infrastructure.Logger.Factories;

namespace Implementation.Logger.Factories
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Guid id)
        {
            return new Logger(id);
        }
    }
}