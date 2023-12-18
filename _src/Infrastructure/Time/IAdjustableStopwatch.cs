using System;
using Infrastructure.Time.Listeners;

namespace Infrastructure.Time;

public interface IAdjustableStopwatch
{
    bool IsRunning { get; }
    TimeSpan Elapsed { get; }
    void Start();
    void Stop();
    void Reset();
    void AttachListener(ITickListener listener);

}