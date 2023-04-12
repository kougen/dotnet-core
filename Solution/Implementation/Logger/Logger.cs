using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Infrastructure.Logger;

namespace Implementation.Logger
{
    internal class Logger : ILogger
    {
        private readonly Guid _id;
        private readonly string _logPath;

        public Logger(Guid id, string logPath)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(id));
            }

            _id = id;
            _logPath = logPath ?? throw new ArgumentNullException(nameof(logPath));

            if (!File.Exists(_logPath))
            {
                using (var streamWriter = new StreamWriter(File.Open(_logPath, FileMode.Create), Encoding.ASCII))
                {
                    streamWriter.AutoFlush = true;
                    streamWriter.WriteLine(_id.ToString());
                }
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
            string line;
            using (var reader = new StreamReader(_logPath))
            {
                line = reader.ReadLine();
            }

            if (Guid.TryParse(line, out var testId) && testId.Equals(_id))
            {
                streamWriter = new StreamWriter(_logPath, true, Encoding.ASCII);
            }
            else
            {
                streamWriter = new StreamWriter(File.Open(_logPath, FileMode.Create), Encoding.ASCII);
                streamWriter.WriteLine(_id.ToString());
            }
        }
    }
}
