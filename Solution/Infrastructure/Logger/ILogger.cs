using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Logger
{
    public interface ILogger : IDisposable
    {
        string LogWrite(string message);
        Task<string> LogWriteAsync(string message);
        
        string LogWriteLine(string message);
        Task<string> LogWriteLineAsync(string message);
        
        string GetLoggedContent();
        Task<string> GetLoggedContentAsync();
    }
}
