using System.Collections.Generic;

namespace Infrastructure.Logger
{
    public interface ILogger
    {
        string LogWrite(string message);
        string LogWriteLine(string message);
        string GetLoggedContent();
    }
}
