using System.Collections.Generic;

namespace Infrastructure.IO
{
    public interface IDataParser
    {
        /// <summary>
        /// Used to specify the parser method for the <c>ReadLine()</c> method.
        /// </summary>
        /// <typeparam name="T">Can be any ValueType.</typeparam>
        delegate bool TryParseHandler<T>(string value, out T result);
        
        /// <summary>
        /// Converts the string input into a single object
        /// </summary>
        /// <param name="input">The string to be converted.</param>
        /// <param name="handler">The TryParseHandler can be a built in TryParse or a custom one.</param>
        /// <param name="isOkay">The out result of the successfulness of the conversion.</param>
        /// <typeparam name="T">Can be any ValueType.</typeparam>
        /// <returns>The converted value</returns>
        T? TryParse<T>(string input, TryParseHandler<T> handler, out bool isOkay);
        
        /// <summary>
        /// Converts the string into a list based on the specified separators
        /// </summary>
        /// <param name="input">The string to be converted.</param>
        /// <param name="handler">The TryParseHandler can be a built in TryParse or a custom one.</param>
        /// <param name="separator">A single separators to split the string</param>
        /// <param name="separators">More characters to split the string at.</param>
        /// <typeparam name="T">Can be any ValueType.</typeparam>
        /// <returns>The converted IEnumerable of the type</returns>
        IEnumerable<T> TryParse<T>(string input, TryParseHandler<T> handler, char separator, params char[] separators);
        
        // NOTE: The current implementation only works for the environment specific line-breaks
        /// <summary>
        /// Converts the string into a list based on the specified separators. Used for multi line conversion.
        /// </summary>
        /// <param name="input">The string to be converted.</param>
        /// <param name="handler">The TryParseHandler can be a built in TryParse or a custom one.</param>
        /// <param name="separator">A single separators to split the string</param>
        /// <param name="separators">More characters to split the string at.</param>
        /// <typeparam name="T">Can be any ValueType.</typeparam>
        /// <returns>The converted 2D IEnumerable of the type</returns>
        IEnumerable<IEnumerable<T>> MultiTryParse<T>(string input, TryParseHandler<T> handler, char separator, params char[] separators);
    }
}
