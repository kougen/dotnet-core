using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Time.Factories;
using Infrastructure.Time.Listeners;

namespace Infrastructure.Time
{
    public interface IStopwatch : IAdjustableStopwatch, IDisposable
    {
        void Wait(int periodInMilliseconds, ITickListener listener);
        Task WaitAsync(int periodInMilliseconds, ITickListener listener);
        void PeriodicOperation(int periodInMilliseconds, ITickListener listener, CancellationToken cancellationToken);
        IPeriodicStopwatchFactory GetPeriodicStopwatchFactory();
        void RegisterStopwatch(IPeriodicStopwatch stopwatch);
        void UnregisterStopwatch(IPeriodicStopwatch stopwatch);
    }
}
