using System;

namespace Infrastructure.Time
{
    public interface IPeriodicStopwatch : IAdjustableStopwatch, IDisposable
    {
        void ChangePeriod(int periodInMilliseconds);
        void Resume();
    }
}
