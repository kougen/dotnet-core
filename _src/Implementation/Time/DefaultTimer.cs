using System;
using System.Diagnostics;
using GameFramework.Time;
using Infrastructure.Time;
using Infrastructure.Time.Listeners;

namespace GameFramework.Impl.Time
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
