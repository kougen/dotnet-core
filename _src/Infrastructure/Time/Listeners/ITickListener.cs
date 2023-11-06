using System;

namespace Infrastructure.Time.Listeners
{
    public interface ITickListener
    {
        void RaiseTick(int round);
        
        TimeSpan ElapsedTime { get; set; }
    }
}
