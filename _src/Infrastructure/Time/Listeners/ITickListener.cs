using System;

namespace Infrastructure.Time.Listeners
{
    public interface ITickListener
    {
        void RaiseTick(int round);
        void RaiseTick(IStopwatch initiator, string name, int round);
        TimeSpan ElapsedTime { get; set; }
    }
}
