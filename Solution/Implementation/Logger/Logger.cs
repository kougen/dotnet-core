using System;
using System.IO;
using System.Text;
using Infrastructure.Logger;

namespace Implementation.Logger
{
    internal class Logger : ILogger
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
            // if (!File.Exists(_logPath))
            // {
            //     using (var streamWriter = new StreamWriter(File.Open(_logPath, FileMode.Create), Encoding.ASCII))
            //     {
            //         streamWriter.AutoFlush = true;
            //     }
            // }
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

        protected virtual void Dispose(bool disposing)
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
                CreateFileStreamAndWriter(out var streamWriter);

                streamWriter.Write(message);
                streamWriter.Close();

                return message;
            }

            public string LogWriteLine(string message)
            {
                CreateFileStreamAndWriter(out var streamWriter);

                message = $"{message}\n";

                streamWriter.Write(message);
                streamWriter.Close();

                return message;
            }

            public string GetLoggedContent()
            {
                var contentStr = File.ReadAllText(_logPath, Encoding.ASCII);

                contentStr = contentStr.Replace($"{_id}\r\n", "");

                return contentStr;
            }

            private void CreateFileStreamAndWriter(out StreamWriter streamWriter)
            {
                // string line;
                // using (var reader = new StreamReader(_logPath))
                // {
                //     line = reader.ReadLine();
                // }

                streamWriter =
                    File.Exists(_logPath)
                        ? new StreamWriter(_logPath, true, Encoding.ASCII)
                        : new StreamWriter(File.Open(_logPath, FileMode.Create), Encoding.ASCII);
                // new StreamWriter(File.Create(_logPath, 4096, FileOptions.DeleteOnClose),  Encoding.ASCII);

                // streamWriter.WriteLine(_id.ToString());
            }
        }
    }