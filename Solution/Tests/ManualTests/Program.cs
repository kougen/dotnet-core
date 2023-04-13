using System;
using System.Collections.Generic;
using System.Linq;
using Implementation.IO.Factories;
using Implementation.Logger.Factories;
using Implementation.Navigator;
using Implementation.Navigator.Factories;
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
            
            var r = _reader.ReadLine<int>(int.TryParse).FirstOrDefault();
            _writer.WriteLine(r.ToString());
            
            var nav = new NavigatorFactory().CreateNavigator(_writer, 
                new List<INavigatorElement>() { new NavigatorElement("asd", () => { }) });
            
            nav.Show();

            // var people = _reader.ReadLine<TestPersonClass>(
            //     TestPersonClass.TryParse, 
            //     "Kerem az emberek adatait tabulatorral elvalasztva\nNev\tKor\tMagassag\t\n");
            //
            // foreach (var person in people)
            // {
            //     Console.WriteLine($"{person.Name}: {person.Age} eves es {person.Height}cm magas");
            // }
        }
    }
}
