using System;
using Implementation.IO.Factories;
using Implementation.Logger.Factories;
using Infrastructure.IO;

namespace Core
{
    public static class Bootstrapper
    {
        public static void Initialize(out IReader reader)
        {
            var id = Guid.NewGuid();
            using var logger = new LoggerFactory().CreateLogger(id);
            var ioFactory = new IOFactory();
            reader = ioFactory.CreateReader(logger, ioFactory.CreateWriter(logger));
        }
        
        public static void Initialize(out IWriter writer, out IReader reader)
        {
            var id = Guid.NewGuid();
            using var logger = new LoggerFactory().CreateLogger(id);
            var ioFactory = new IOFactory();
            writer = ioFactory.CreateWriter(logger);
            reader = ioFactory.CreateReader(logger, writer);
        }
    }
}
