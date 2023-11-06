using System;
using Infrastructure.Time;
using Infrastructure.Time.Listeners;

namespace Implementation.Time
{
    internal class DefaultTimer : ITimer
    {
        public TimeSpan Remaining { get; }
        
        public void AddListener(ITickListener listener)
        {
            throw new NotImplementedException();
        }
    }
}
