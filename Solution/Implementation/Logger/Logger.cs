using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Implementation.Commands;
using Implementation.Logger.Commands;
using Infrastructure.Commands;
using Infrastructure.Logger;

namespace Implementation.Logger
{
    internal sealed class Logger : ILogger
    {
        private bool _isDisposed;
        private readonly Guid _id;
        private readonly string _logPath;

        public Logger(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(id));
            }

            _id = id;
            _logPath = $"{id.ToString().Replace("-", "")}.txt";
            _isDisposed = false;
        }
        
        public Logger(Guid id, string logPath)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(id));
            }

            _id = id;
            _logPath = logPath ?? throw new ArgumentNullException(nameof(logPath));
            _isDisposed = false;
        }

        ~Logger()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    File.Delete(_logPath);
                }

                _isDisposed = true;
            }
        }

        public string LogWrite(string message)
        {
            return LogWriteAsync(message).Result;
        }

        public async Task<string> LogWriteAsync(string message)
        {
            ICommandInvoker<string> commandInvoker = new CommandInvoker<string>(new LogCommand(_logPath, message));
            var result = await commandInvoker.ExecuteCommand();
            return result;
        }

        public string LogWriteLine(string message)
        {
            return LogWriteLineAsync(message).Result;
        }

        public async Task<string> LogWriteLineAsync(string message)
        {
            message = $"{message}\n";
            var result = await LogWriteAsync(message);
            return result;
        }

        public string GetLoggedContent()
        {
            return GetLoggedContentAsync().Result;
        }

        public async Task<string> GetLoggedContentAsync()
        {
            ICommandInvoker<string> commandInvoker = new CommandInvoker<string>(new GetLogCommand(_logPath));
            return await commandInvoker.ExecuteCommand();
        }
    }
}