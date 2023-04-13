using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Infrastructure;
using Infrastructure.IO;
using Infrastructure.Logger;

namespace Implementation.IO
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

        public IEnumerable<T> ReadLine<T>(StreamReader streamReader, IReader.TryParseHandler<T> handler, out bool isOkay)
        {
            Console.SetIn(streamReader);
            isOkay = false;

            var rawInput = ReadLine(streamReader);
            _logger.LogWrite($"{rawInput}\n");
            var lines = rawInput.Split('\n').ToList();
            lines.RemoveAll(string.IsNullOrWhiteSpace);
            lines.RemoveAll(c => c is "\r\n" or "\n" or "\r" or "\n\r");

            var convertedLines = new List<T>();
            foreach (var line in lines)
            {
                isOkay = handler(line, out var result);
                if (isOkay)
                {
                    convertedLines.Add(result);
                }
            }

            return convertedLines;
        }

        public IEnumerable<IEnumerable<T>> ReadLine<T>(StreamReader streamReader, IReader.TryParseHandler<T> handler, char separator)
        {
            return ReadLine(streamReader, handler, new[] { separator });
        }

        public IEnumerable<IEnumerable<T>> ReadLine<T>(StreamReader streamReader, IReader.TryParseHandler<T> handler, char[] separators)
        {
            Console.SetIn(streamReader);

            var lines = new List<string>();
            while (!streamReader.EndOfStream)
            {
                var rawInput = ReadLine(streamReader);
                _logger.LogWrite($"{rawInput}\n");
                lines.Add(rawInput);
            }

            var convertedList = new List<IEnumerable<T>>();
            foreach (var line in lines)
            {
                var innerList = new List<T>();

                var elements = line.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();
                elements.RemoveAll(c => c is "\r\n" or "\n" or "\r" or "\n\r");
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

        public IEnumerable<T> ReadLine<T>(StreamReader streamReader, IReader.TryParseHandler<T> handler)
        {
            return ReadLine(streamReader, handler, out _);
        }

        public string ReadLine(StreamReader streamReader)
        {
            var fromConsole = !(streamReader.BaseStream.GetType() == typeof(FileStream));
            Console.SetIn(streamReader);
            var sb = new StringBuilder();
            var test1 = "";
            while ((test1 is not "\r\n" or "\n\r" && !streamReader.EndOfStream) || fromConsole)
            {
                var readInt = streamReader.Read();
                var readChar = (char)readInt;
                sb.Append(readChar);
                test1 = LastChars(sb, 2);
                var test2 = LastChars(sb, 3);
                
                if (fromConsole && test2 == "\r\n\r")
                {
                    return sb.ToString();
                }
                
                if(fromConsole && test1 is "\r\n" or "\n\r")
                {
                    Console.WriteLine("To finish the input press enter again!");
                }
                else if (!fromConsole && test1 is "\r\n" or "\n\r" )
                {
                    return sb.ToString();
                }
            }

            return sb.ToString();
        }

        /// <summary>Returns the last 'n' chars of a StringBuilder. </summary>
        private static string LastChars(StringBuilder sb, int n)
        {
            n = Math.Min(n, sb.Length);
            var chars = new char[n];

            for (var i = 0; i < n; ++i)
                chars[i] = sb[i + sb.Length - n];

            return new string(chars);
        }
    }
}
