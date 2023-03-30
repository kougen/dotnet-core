using System;
using System.IO;
using System.Linq;
using Implementation.Logger;
using Implementation.StandardIOManager;

namespace ManualTests
{
    class Program
    {
        private static Reader _reader;
        static void Main(string[] args)
        {
            var id = Guid.NewGuid();
            var logger = new Logger(id, "testLog.txt");
            
            _reader = new Reader(logger, new Writer(logger));
            
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
