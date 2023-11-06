using System;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Commands;

namespace Implementation.Logger.Commands
{
    internal class ClearLogCommand : ICommand<bool>
    {
        private readonly string _path;

        public ClearLogCommand(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            CanExecute = true;
        }
        public bool CanExecute { get; private set; }
        public async Task<bool> Execute()
        {
            CanExecute = false;
            if (File.Exists(_path))
            {
                await File.WriteAllTextAsync(_path,string.Empty);
                CanExecute = true;
                return true;
            }

            return await Task.FromResult(false);
        }
    }
}