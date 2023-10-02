using System;
using System.IO;
using System.Reflection;
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
        private string _logDir;
        private string _logPath;

        public Logger() : this(Guid.NewGuid())
        { }
        
        public Logger(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(id));
            }

            _id = id;
            _logDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            _logPath = Path.Join(_logDir, GetIdFileName());
            _isDisposed = false;
            InitFile();
        }
        
        public Logger(Guid id, string logPath)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(logPath))
            {
                throw new ArgumentNullException(nameof(logPath));
            }

            _id = id;
            _logDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            _logPath = Path.Join(_logDir, logPath);
            _isDisposed = false;
            InitFile();
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

        public async Task ClearLogs()
        {
            ICommandInvoker<bool> commandInvoker = new CommandInvoker<bool>(new ClearLogCommand(_logPath));
            await commandInvoker.ExecuteCommand();
        }

        private void InitFile()
        {
            if (!File.Exists(_logPath))
            {
                try
                {
                    File.Create(_logPath).Close();
                }
                catch (Exception)
                {
                    _logDir = Path.GetTempPath();
                    _logPath = Path.Join(_logDir, GetIdFileName());
                    File.Create(_logPath).Close();
                }
            }
        }

        private string GetIdFileName()
        {
            return $"{_id.ToString().Replace("-", "")}.txt";
        }
    }
}