using System;

namespace Infrastructure.Time
{
    public interface IPeriodicStopwatch : IAdjustableStopwatch, IDisposable
    {
        IStopwatch Parent { get; }
        void ChangePeriod(int periodInMilliseconds);
        void Resume();
    }
}
