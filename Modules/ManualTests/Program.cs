using System;
using Implementation.Core;
using Infrastructure.IO;
using Unity;

namespace ManualTests
{
    class Program
    {
        private static IReader _reader;
        static void Main(string[] args)
        {
            var id = Guid.NewGuid();
            var c = Bootsrapper.GetDefaultContainer("testLog.txt", id);
            _reader = c.Resolve<IReader>();

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
