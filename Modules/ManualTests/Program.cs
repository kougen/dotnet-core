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
            
            var test = _reader.ReadLine<TestPersonClass>(TestPersonClass.TryParse).FirstOrDefault();
            
            var first = test;
            Console.WriteLine(first.Age);
            Console.WriteLine(first.Height);
        }
    }
}
