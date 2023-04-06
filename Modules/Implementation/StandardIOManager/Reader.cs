using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                var rawInput = Console.ReadLine();
                _logger.LogWrite($"{rawInput}\n");
                lines.Add(rawInput);
            }

            var convertedList = new List<IEnumerable<T>>();
            foreach (var line in lines)
            {
                var innerList = new List<T>();

                var elements = line.Split(separator).ToList();
                elements.RemoveAll(string.IsNullOrWhiteSpace);
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

        public IEnumerable<T> ReadLine<T>(IReader.TryParseHandler<T> handler, string prompt)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            var time = DateTime.Now.ToString("HH:mm:ss");
            _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
            var ans = ReadLine(reader, handler, out _);
            return ans;
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

        private string ReadLine()
        {
            var sb = new StringBuilder();
            string blank = new string(' ', Console.WindowWidth - 1);

            while (true)
            {
                var k = Console.ReadKey();

                if((k.Modifiers & ConsoleModifiers.Control) != 0){
                
                }
                switch (k.Key)
                {
                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return sb.ToString();
                    case ConsoleKey.Backspace:
                    {
                        if (sb.Length > 0)
                        {
                            --sb.Length;
                        }
                        break;
                    }
                    case ConsoleKey.Tab:
                        sb.Append('\t');
                        break;
                    default:
                    {
                        if (k.KeyChar != '\0') // Ignore special keys.
                        {
                            sb.Append(k.KeyChar);
                        }
                        break;
                    }
                }

                Console.Write("\r" + blank);
                Console.Write("\r" + sb.ToString());
            }
        }

        /// <summary>Returns the last 'n' chars of a StringBuilder. </summary>
        static string lastChars(StringBuilder sb, int n)
        {
            n = Math.Min(n, sb.Length);
            char[] chars = new char[n];

            for (int i = 0; i < n; ++i)
                chars[i] = sb[i + sb.Length - n];

            return new string(chars);
        }
    }
}
