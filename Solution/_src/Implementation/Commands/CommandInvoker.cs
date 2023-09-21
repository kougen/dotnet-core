using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Commands;

namespace Implementation.Commands
{
    public class CommandInvoker<T> : ICommandInvoker<T>
    {
        private readonly IList<ICommand<T>> _commands;

        public CommandInvoker(ICommand<T> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _commands = new List<ICommand<T>>();
            _commands.Add(command);
        }
        
        public void AddCommand(ICommand<T> command)
        {
            _commands.Add(command);
        }

        public async Task<T> ExecuteCommand()
        {
            if (_commands.Any() && _commands.ToArray()[0].CanExecute)
            {
                var res = await _commands.ToArray()[0].Execute();
                return res;
            }

            return await Task.FromResult<T>(default);
        }

        public async Task<bool> ExecuteCommands()
        {
            foreach (var command in _commands)
            {
                if (command.CanExecute)
                {
                    await command.Execute();
                }
            }

            return await Task.FromResult(true);
        }
    }
}