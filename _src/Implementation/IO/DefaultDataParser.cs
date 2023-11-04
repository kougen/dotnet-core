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

        public IEnumerable<T> TryParse<T>(string input, IDataParser.TryParseHandler<T> handler, out bool isOkay, char separator, params char[] separators)
        {
            isOkay = false;
            separators = new[]
            {
                separator
            }.Concat(separators).ToArray();
            
            var lines = input.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();
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
        
        public IEnumerable<IEnumerable<T>> MultiTryParse<T>(string input, IDataParser.TryParseHandler<T> handler, out bool isOkay, char separator, params char[] separators)
        {
            isOkay = false;
            var convertedLines = new List<IEnumerable<T>>();
            var lines = input.Split(Environment.NewLine);

            foreach (var line in lines)
            {
                convertedLines.Add(TryParse(line, handler, out isOkay, separator, separators));
            }
            
            return convertedLines;
        }
    }
}
