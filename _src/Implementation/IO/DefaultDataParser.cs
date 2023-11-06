using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.IO;

namespace Implementation.IO
{
    internal class DefaultDataParser : IDataParser
    {
        public T TryParse<T>(string input, IDataParser.TryParseHandler<T> handler, out bool isOkay)
        {
            isOkay = handler(input, out var result);
            return result;
        }

        public IEnumerable<T> TryParse<T>(string input, IDataParser.TryParseHandler<T> handler, char separator, params char[] separators)
        {
            separators = new[]
            {
                separator
            }.Concat(separators).ToArray();
            
            var lines = input.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.RemoveAll(c => c is "\r\n" or "\n" or "\r" or "\n\r");

            var convertedLines = new List<T>();
            foreach (var line in lines)
            {
                var isOkay = handler(line, out var result);
                if (isOkay)
                {
                    convertedLines.Add(result);
                }
            }

            return convertedLines;
        }
        
        public IEnumerable<IEnumerable<T>> MultiTryParse<T>(string input, IDataParser.TryParseHandler<T> handler, char separator, params char[] separators)
        {
            var lines = input.Split(Environment.NewLine);

            return lines.Select(line => TryParse(line, handler, separator, separators)).ToList();
        }
    }
}
