using System.Threading;

namespace Infrastructure.Module
{
    public interface ICancellableModule
    {
        CancellationTokenSource Source { get; }
    }
}
