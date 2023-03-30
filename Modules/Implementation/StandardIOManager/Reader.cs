using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Infrastructure;
using Infrastructure.IO;
using Infrastructure.Logger;

namespace Implementation.StandardIOManager
{
    internal class Reader : IReader
    {
        
        private readonly ILogger _logger;
        private readonly IWriter _writer;

        public Reader(ILogger logger, IWriter writer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }
        
        public IEnumerable<T> ReadLine<T>(StreamReader reader, IReader.TryParseHandler<T> handler, out bool isOkay)
        {
            Console.SetIn(reader);
            isOkay = false;
            var lines = new List<string>();
            while (!reader.EndOfStream)
            {
                var rawInput = Console.ReadLine();
                _logger.LogWrite($"{rawInput}\n");
                lines.Add(rawInput);
            }

            var convertedLines = new List<T>();
            foreach (var line in lines)
            {
                isOkay = handler(line, out var result);
                if (isOkay)
                {
                    convertedLines.Add(result);
                }
                else
                {
                    throw new ArgumentException("Error in line element conversion!");
                }
            }

            return convertedLines;
        }

        public IEnumerable<IEnumerable<T>> ReadLine<T>(StreamReader reader, IReader.TryParseHandler<T> handler, char separator)
        {
            Console.SetIn(reader);

            var lines = new List<string>();
            while (!reader.EndOfStream)
            {
                var rawInput = Console.ReadLine()!;
                _logger.LogWrite($"{rawInput}\n");
                lines.Add(rawInput);
            }

            var convertedList = new List<IEnumerable<T>>();
            foreach (var line in lines)
            {
                var innerList = new List<T>();

                var elements = line.Split(separator).ToList();
                foreach (var element in elements)
                {
                    if (handler(element, out var result))
                    {
                        innerList.Add(result);
                    }
                    else
                    {
                        throw new ArgumentException("Error in line element conversion!");
                    }
                }
                convertedList.Add(innerList);
            }
            return convertedList;
        }

        public T ReadLine<T>(IReader.TryParseHandler<T> handler, string prompt)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            var time = DateTime.Now.ToString("HH:mm:ss");
            _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
            var ans = ReadLine(reader, handler, out _);
            return ans.FirstOrDefault();
        }
        
        public T ReadLine<T>(IReader.TryParseHandler<T> handler, string prompt, string errorMsg)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            while (true)
            {
                var time = DateTime.Now.ToString("HH:mm:ss");
                _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
                var ans = ReadLine(reader, handler, out var isCorrect);
        
                if (isCorrect)
                {
                    return ans.FirstOrDefault();
                }
        
                _writer.WriteLine(MessageSeverity.Error, $"{errorMsg} | (Invalid type: {typeof(T)})!");
            }
        }
        
        public IEnumerable<T> ReadLine<T>(IReader.TryParseHandler<T> handler)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            return ReadLine(reader, handler);
        }
        
        public IEnumerable<T> ReadLine<T>(StreamReader reader, IReader.TryParseHandler<T> handler)
        {
            return ReadLine(reader, handler, out _);
        }
    }
}
