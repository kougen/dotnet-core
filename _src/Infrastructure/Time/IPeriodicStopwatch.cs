using System;
using Infrastructure.Time.Listeners;

namespace Infrastructure.Time
{
    public interface IPeriodicStopwatch
    {
        TimeSpan Elapsed { get; }

        void Start();
        void ChangePeriod(int periodInMilliseconds);
        void Stop();
        void Resume();
        void Reset();
        
        void AttachListener(ITickListener listener);
    }
}
