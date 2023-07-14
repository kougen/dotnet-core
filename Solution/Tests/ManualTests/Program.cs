using System;
using System.Collections.Generic;
using Implementation.IO.Factories;
using Implementation.Logger.Factories;
using Implementation.Navigator;
using Implementation.Navigator.Factories;
using Infrastructure;
using Infrastructure.IO;
using Infrastructure.Navigator;

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
            
            _writer.PrintSystemDetails("joshika39", 
                "Joshua Hegedus", 
                "YQMHWO",
                "Name of project",
                "Some long description.");
            var test = _reader.ReadLine<int>(int.TryParse, "Kerek egy szamot");
            var test2 = new List<INavigatorElement<int>>
            {
                new NavigatorElement<int>("asd", 123),
                new NavigatorElement<int>("zxc", 2),
                new NavigatorElement<int>("qwe", 32),
                new NavigatorElement<int>("ret", 43)
            };
            var nav = new NavigatorFactory().CreateNavigator<int>(_writer);
            nav.UpdateItems(test2);
            var res = nav.Show();
            _writer.WriteLine(MessageSeverity.Success, $"Res: {res}");
        }
    }
}
