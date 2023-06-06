using System;
using Implementation.IO.Factories;
using Implementation.Logger.Factories;
using Infrastructure.IO;

namespace ManualTests
{
    internal static class Program
    {
        private static IReader _reader;
        private static IWriter _writer;
        static void Main(string[] args)
        {
            var id = Guid.NewGuid();
            using var logger = new LoggerFactory().CreateLogger(id);
            
            var ioFactory = new IOFactory();
            _writer = ioFactory.CreateWriter(logger);
            _reader = ioFactory.CreateReader(logger, _writer);
            _writer.WriteLine("TestMessage");
            _writer.PrintSystemDetails("joshika39", 
                "Joshua Hegedus", 
                "YQMHWO",
                "Name of project",
                "Some long description.");
        }
    }
}
