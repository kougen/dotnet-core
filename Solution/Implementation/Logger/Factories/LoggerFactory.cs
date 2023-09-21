using System;
using Infrastructure.Logger;
using Infrastructure.Logger.Factories;

namespace Implementation.Logger.Factories
{
    internal class LoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger()
        {
            return new Logger();
        }
        
        public ILogger CreateLogger(Guid id)
        {
            return new Logger(id);
        }
    }
}