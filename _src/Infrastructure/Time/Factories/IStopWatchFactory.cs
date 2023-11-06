using System.Threading;

namespace Infrastructure.Time.Factories
{
    public interface IStopWatchFactory
    {
        IStopwatch CreateStopwatch(CancellationToken token);
    }
}
