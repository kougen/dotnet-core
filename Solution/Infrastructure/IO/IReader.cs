using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure.IO
{
    public interface IReader
    {
        /// <summary>
        /// Used to specify the parser method for the <c>ReadLine()</c> method.
        /// </summary>
        /// <typeparam name="T">Can be any ValueType.</typeparam>
        delegate bool TryParseHandler<T>(string value, out T result);

        /// <summary>
        /// This is the core method. It is using <c>Console.ReadLine</c> method, to get the data
        /// </summary>
        /// <param name="streamReader"></param>
        /// <param name="handler">A type parser method</param>
        /// <param name="isOkay"></param>
        /// <param name="separators"></param>
        /// <example>
        /// This shows a conversion to <c>int</c>.
        /// <code>var age = ReadLine&lt;int&gt;(int.TryParse);</code>
        /// </example>
        /// <typeparam name="T">The target value converted from <c>Console.Readline()</c></typeparam>
        /// <seealso cref="Console.ReadLine()"/>
        /// <returns>The converted Type passed with the type parameter.</returns>
        /// <exception cref="InvalidOperationException">Exception being thrown when the conversion is unsuccessful.</exception>
        IEnumerable<T> ReadLine<T>(StreamReader streamReader, TryParseHandler<T> handler, out bool isOkay, params char[] separators);

        
        #region ReadLine
        T ReadLine<T>(TryParseHandler<T> handler, string prompt);
        IEnumerable<T> ReadLine<T>(TryParseHandler<T> handler, string prompt, params char[] separators);
        T ReadLine<T>(TryParseHandler<T> handler, string prompt, string errorMsg);
        IEnumerable<T> ReadLine<T>(TryParseHandler<T> handler, string prompt, string errorMsg, params char[] separators);
        T ReadLine<T>(TryParseHandler<T> handler);
        IEnumerable<T> ReadLine<T>(TryParseHandler<T> handler, params char[] separators);
        T ReadLine<T>(StreamReader streamReader, TryParseHandler<T> handler);
        IEnumerable<T> ReadLine<T>(StreamReader streamReader, TryParseHandler<T> handler, params char[] separators);
        #endregion
        
        #region ReadAllLines
        string ReadAllLines(string prompt);
        IEnumerable<T> ReadAllLines<T>(StreamReader streamReader, TryParseHandler<T> handler);
        IEnumerable<IEnumerable<T>> ReadAllLines<T>(StreamReader streamReader, TryParseHandler<T> handler, params char[] separators);
        #endregion

        
    }
}
