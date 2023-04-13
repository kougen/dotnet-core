using System;
using System.Collections.Generic;

namespace Infrastructure.Logger
{
    public interface ILogger : IDisposable
    {
        string LogWrite(string message);
        string LogWriteLine(string message);
        string GetLoggedContent();
    }
}
