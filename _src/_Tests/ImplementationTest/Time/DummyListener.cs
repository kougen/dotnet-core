using System;
using Infrastructure.Time;
using Infrastructure.Time.Listeners;

namespace ImplementationTest.Time
{
    internal class DummyListener : ITickListener
    {
        public string Name { get; }

        public DummyListener(string name, IPeriodicStopwatch periodicStopwatch)
        {
            Name = name;
            periodicStopwatch.AttachListener(this);
        }
        
        public void RaiseTick(int round)
        {
            Console.WriteLine($"{Name} -> {round}, time: {ElapsedTime}");
        }

        public TimeSpan ElapsedTime { get; set; }
    }
}
