using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Commands;

namespace Implementation.Logger.Commands
{
    public class LogCommand : ICommand<string>
    {
        private readonly string _filePath;
        private readonly string _content;

        public LogCommand(string filePath, string content)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _content = content ?? throw new ArgumentNullException(nameof(content));
            CanExecute = true;
        }

        public bool CanExecute { get; private set; }

        public async Task<string> Execute()
        {
            CanExecute = false;
            
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException("Log file does not exists!");
            }

            using (var streamWriter = new StreamWriter(_filePath, true, Encoding.ASCII))
            {
                await streamWriter.WriteAsync(_content);
            }

            CanExecute = true;

            return _content;
        }
    }
}