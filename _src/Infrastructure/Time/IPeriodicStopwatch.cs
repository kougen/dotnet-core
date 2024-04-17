using System;
using Infrastructure.Time.Listeners;

namespace Infrastructure.Time
{
    public interface IPeriodicStopwatch : IAdjustableStopwatch, INamedStopwatch, IDisposable
    {
        IStopwatch Parent { get; }
        void ChangePeriod(int periodInMilliseconds);
        void Resume();
    }
}
