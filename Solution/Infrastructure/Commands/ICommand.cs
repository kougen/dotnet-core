using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public interface ICommand<T>
    {
        bool CanExecute { get; }
        Task<T> Execute();
    }
}