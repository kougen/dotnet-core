using System.Threading;

namespace Infrastructure.Application
{
    public interface ILifeCycleManager
    {
        CancellationTokenSource Source { get; }
        CancellationToken Token { get; }
    }
}
