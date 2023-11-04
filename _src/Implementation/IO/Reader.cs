using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IDataParser _dataParser;

        public Reader(ILogger logger, IWriter writer, IDataParser dataParser)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _dataParser = dataParser ?? throw new ArgumentNullException(nameof(dataParser));
        }

        public IEnumerable<T> ReadLine<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler,
            out bool isOkay, char separator, params char[] separators)
        {
            Console.SetIn(streamReader);

            var rawInput = ReadLine(streamReader, out var isConsole);
            if (isConsole)
            {
                _logger.LogWrite($"{rawInput}\n");
            }
            
            return _dataParser.TryParse(rawInput, handler, out isOkay, separator, separators);
        }
        
        public T? ReadLine<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler, out bool isOkay)
        {
            Console.SetIn(streamReader);

            var rawInput = ReadLine(streamReader, out var isConsole);
            if (isConsole)
            {
                _logger.LogWrite($"{rawInput}\n");
            }
            
            
            return _dataParser.TryParse(rawInput, handler, out isOkay);
        }

        public T? ReadLine<T>(IDataParser.TryParseHandler<T> handler, string prompt, out bool isOkay)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            var time = DateTime.Now.ToString("HH:mm:ss");
            _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
            var ans = ReadLine(reader, handler, out isOkay);
            return ans;
        }

        public IEnumerable<T> ReadLine<T>(IDataParser.TryParseHandler<T> handler, string prompt, char separator, params char[] separators)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            var time = DateTime.Now.ToString("HH:mm:ss");
            _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
            var ans = ReadLine(reader, handler, out _, separator, separators);
            return ans;
        }

        public string ReadLine(string prompt, string errorMsg)
        {
            return ReadLine<string>(Dummy, prompt, errorMsg);
        }

        public T ReadLine<T>(IDataParser.TryParseHandler<T> handler, string prompt, string errorMsg)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            while (true)
            {
                var time = DateTime.Now.ToString("HH:mm:ss");
                _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
                var ans = ReadLine(reader, handler, out var isCorrect);

                if (isCorrect && ans is not null)
                {
                    return ans;
                }

                _writer.WriteLine(MessageSeverity.Error, $"{errorMsg} | (Invalid type: {typeof(T)})!");
            }
        }

        public IEnumerable<T> ReadLine<T>(IDataParser.TryParseHandler<T> handler, string prompt, string errorMsg,
            char separator, params char[] separators)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            while (true)
            {
                var time = DateTime.Now.ToString("HH:mm:ss");
                _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
                var ans = ReadLine(reader, handler, out var isCorrect, separator, separators);

                if (isCorrect)
                {
                    return ans;
                }

                _writer.WriteLine(MessageSeverity.Error, $"{errorMsg} | (Invalid type: {typeof(T)})!");
            }
        }

        public T? ReadLine<T>(IDataParser.TryParseHandler<T> handler, out bool isOkay)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            return ReadLine(reader, handler, out isOkay);
        }

        public IEnumerable<T> ReadLine<T>(IDataParser.TryParseHandler<T> handler, char separator, params char[] separators)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            return ReadLine(reader, handler, separator, separators);
        }

        public IEnumerable<T> ReadLine<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler,
            char separator, params char[] separators)
        {
            return ReadLine(streamReader, handler, out _, separator, separators);
        }

        public string ReadAllLines(string prompt)
        {
            var streamReader = new StreamReader(Console.OpenStandardInput());
            var time = DateTime.Now.ToString("HH:mm:ss");
            _writer.WriteLine(MessageSeverity.Info,
                "To finish entering your long text input enter the 'end of file' character. (Usually Ctrl + Z on windows)");
            _writer.Write(Constants.EscapeColors.CYAN, $"[  INPUT: {time}] {prompt}");
            return ReadLine(streamReader, out _, true);
        }

        public IEnumerable<T> ReadAllLines<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler)
        {
            var items = new List<T>();
            while (!streamReader.EndOfStream)
            {
                var item = ReadLine(streamReader, handler, out var isOkay);
                if (isOkay && item is not null)
                {
                    items.Add(item);  
                }
            }

            return items;
        }

        public IEnumerable<IEnumerable<T>> ReadAllLines<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler, char separator, params char[] separators)
        {
            var items = new List<IEnumerable<T>>();
            while (!streamReader.EndOfStream)
            {
                items.Add(ReadLine(streamReader, handler, separator, separators));
            }

            return items;
        }

        public static string ReadLine(StreamReader streamReader, out bool fromConsole, bool useEndOfFile=false)
        {
            fromConsole = streamReader.BaseStream.GetType() != typeof(FileStream);
            Console.SetIn(streamReader);
            var sb = new StringBuilder();
            while (!streamReader.EndOfStream)
            {
                var readInt = streamReader.Read();
                var readChar = (char)readInt;
                sb.Append(readChar);
                var test1 = LastChars(sb, 2);


                if (!useEndOfFile && test1 is "\r\n" or "\n\r")
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
        
        private bool Dummy(string line, out string res)
        {
            res = line;
            return true;
        }
    }
}