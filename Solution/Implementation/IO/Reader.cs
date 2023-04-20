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

        public IEnumerable<T> ReadLine<T>(StreamReader streamReader, IReader.TryParseHandler<T> handler,
            out bool isOkay, params char[] separators)
        {
            Console.SetIn(streamReader);
            isOkay = false;

            var rawInput = ReadLine(streamReader);
            _logger.LogWrite($"{rawInput}\n");
            var lines = rawInput.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();
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

        public T ReadLine<T>(StreamReader streamReader, IReader.TryParseHandler<T> handler, out bool isOkay)
        {
            Console.SetIn(streamReader);
            isOkay = false;

            var rawInput = ReadLine(streamReader);
            _logger.LogWrite($"{rawInput}\n");
            isOkay = handler(rawInput, out var result);
            
            return isOkay ? result : default;
        }

        public T ReadLine<T>(IReader.TryParseHandler<T> handler, string prompt)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            var time = DateTime.Now.ToString("HH:mm:ss");
            _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
            var ans = ReadLine(reader, handler, out _);
            return ans;
        }

        public IEnumerable<T> ReadLine<T>(IReader.TryParseHandler<T> handler, string prompt, params char[] separators)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            var time = DateTime.Now.ToString("HH:mm:ss");
            _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
            var ans = ReadLine(reader, handler, out _, separators);
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
                    return ans;
                }

                _writer.WriteLine(MessageSeverity.Error, $"{errorMsg} | (Invalid type: {typeof(T)})!");
            }
        }

        public IEnumerable<T> ReadLine<T>(IReader.TryParseHandler<T> handler, string prompt, string errorMsg,
            params char[] separators)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            while (true)
            {
                var time = DateTime.Now.ToString("HH:mm:ss");
                _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
                var ans = ReadLine(reader, handler, out var isCorrect, separators);

                if (isCorrect)
                {
                    return ans;
                }

                _writer.WriteLine(MessageSeverity.Error, $"{errorMsg} | (Invalid type: {typeof(T)})!");
            }
        }

        public T ReadLine<T>(IReader.TryParseHandler<T> handler)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            return ReadLine(reader, handler);
        }

        public IEnumerable<T> ReadLine<T>(IReader.TryParseHandler<T> handler, params char[] separators)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            return ReadLine(reader, handler, separators);
        }

        public T ReadLine<T>(StreamReader streamReader, IReader.TryParseHandler<T> handler)
        {
            return ReadLine(streamReader, handler, out _);
        }

        public IEnumerable<T> ReadLine<T>(StreamReader streamReader, IReader.TryParseHandler<T> handler,
            params char[] separators)
        {
            return ReadLine(streamReader, handler, out _, separators);
        }

        public string ReadAllLines(string prompt)
        {
            var streamReader = new StreamReader(Console.OpenStandardInput());
            var time = DateTime.Now.ToString("HH:mm:ss");
            _writer.WriteLine(MessageSeverity.Info,
                "To finish entering your long text input enter the 'end of file' character. (Usually Ctrl + Z on windows)");
            _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");


            return ReadLine(streamReader);
        }

        public IEnumerable<T> ReadAllLines<T>(StreamReader streamReader, IReader.TryParseHandler<T> handler)
        {
            var items = new List<T>();
            while (!streamReader.EndOfStream)
            {
                items.Add(ReadLine(streamReader, handler));
            }

            return items;
        }

        public IEnumerable<IEnumerable<T>> ReadAllLines<T>(StreamReader streamReader,
            IReader.TryParseHandler<T> handler, params char[] separators)
        {
            var items = new List<IEnumerable<T>>();
            while (!streamReader.EndOfStream)
            {
                items.Add(ReadLine(streamReader, handler, separators));
            }

            return items;
        }

        public static string ReadLine(StreamReader streamReader)
        {
            var fromConsole = !(streamReader.BaseStream.GetType() == typeof(FileStream));
            Console.SetIn(streamReader);
            var sb = new StringBuilder();
            while (!streamReader.EndOfStream /* || fromConsole*/)
            {
                var readInt = streamReader.Read();
                var readChar = (char)readInt;
                sb.Append(readChar);
                var test1 = LastChars(sb, 2);


                if (!fromConsole && test1 is "\r\n" or "\n\r")
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