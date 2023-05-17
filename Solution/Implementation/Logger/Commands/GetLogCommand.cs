using System;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Commands;

namespace Implementation.Logger.Commands
{
    public class GetLogCommand : ICommand<string>
    {
        private readonly string _filePath;

        public GetLogCommand(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
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

            using (var streamReader = new StreamReader(_filePath))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}