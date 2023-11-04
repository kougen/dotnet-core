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
        
        T? TryParse<T>(string input, TryParseHandler<T> handler, out bool isOkay);
        
        IEnumerable<T> TryParse<T>(string input, TryParseHandler<T> handler, out bool isOkay, char separator, params char[] separators);
    }
}
