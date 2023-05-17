using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public interface ICommandInvoker<T>
    {
        void AddCommand(ICommand<T> command);
        Task<T> ExecuteCommand();
        Task<bool> ExecuteCommands();
    }
}