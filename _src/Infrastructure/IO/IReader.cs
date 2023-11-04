using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure.IO
{
    public interface IReader
    {
        /// <summary>
        /// This is the core method. It is using <c>Console.ReadLine</c> method, to get the data
        /// </summary>
        /// <param name="streamReader"></param>
        /// <param name="handler">A type parser method</param>
        /// <param name="isOkay"></param>
        /// <param name="separator"></param>
        /// <param name="separators"></param>
        /// <example>
        /// This shows a conversion to <c>int</c>.
        /// <code>var age = ReadLine&lt;int&gt;(int.TryParse);</code>
        /// </example>
        /// <typeparam name="T">The target value converted from <c>Console.Readline()</c></typeparam>
        /// <seealso cref="Console.ReadLine()"/>
        /// <returns>The converted Type passed with the type parameter.</returns>
        /// <exception cref="InvalidOperationException">Exception being thrown when the conversion is unsuccessful.</exception>
        IEnumerable<T> ReadLine<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler, out bool isOkay, char separator, params char[] separators);
        T? ReadLine<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler, out bool isOkay);
        
        #region ReadLine
        T? ReadLine<T>(IDataParser.TryParseHandler<T> handler, string prompt, out bool isOkay);
        IEnumerable<T> ReadLine<T>(IDataParser.TryParseHandler<T> handler, string prompt, char separator, params char[] separators);
        
        string ReadLine(string prompt, string errorMsg);
        T ReadLine<T>(IDataParser.TryParseHandler<T> handler, string prompt, string errorMsg);
        IEnumerable<T> ReadLine<T>(IDataParser.TryParseHandler<T> handler, string prompt, string errorMsg, char separator, params char[] separators);
        
        T? ReadLine<T>(IDataParser.TryParseHandler<T> handler, out bool isOkay);
        IEnumerable<T> ReadLine<T>(IDataParser.TryParseHandler<T> handler, char separator, params char[] separators);
        IEnumerable<T> ReadLine<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler, char separator, params char[] separators);
        #endregion
        
        #region ReadAllLines
        string ReadAllLines(string prompt);
        IEnumerable<T> ReadAllLines<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler);
        IEnumerable<IEnumerable<T>> ReadAllLines<T>(StreamReader streamReader, IDataParser.TryParseHandler<T> handler, char separator, params char[] separators);
        #endregion

        
    }
}
