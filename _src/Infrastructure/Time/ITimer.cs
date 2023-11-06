using System;
using Infrastructure.Time.Listeners;

namespace Infrastructure.Time
{
    public interface ITimer
    {
        TimeSpan Remaining { get; }
        void AddListener(ITickListener listener);
    }
}
