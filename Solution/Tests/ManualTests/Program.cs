using System;
using System.IO;
using Implementation.Core;
using Implementation.StandardIOManager;
using Infrastructure.IO;
using Infrastructure.Logger;
using Unity;

namespace ManualTests
{
    class Program
    {
        private static IReader _reader;
        private static IWriter _writer;
        static void Main(string[] args)
        {
            var id = Guid.NewGuid();
            var c = Bootsrapper.GetDefaultContainer("testLog.txt", id);
            _reader = c.Resolve<IReader>();
            _writer = c.Resolve<IWriter>();

            var people = _reader.ReadLine<TestPersonClass>(
                TestPersonClass.TryParse, 
                "Kerem az emberek adatait tabulatorral elvalasztva\nNev\tKor\tMagassag\t\n");
            
            foreach (var person in people)
            {
                Console.WriteLine($"{person.Name}: {person.Age} eves es {person.Height}cm magas");
            }
        }
    }
}
