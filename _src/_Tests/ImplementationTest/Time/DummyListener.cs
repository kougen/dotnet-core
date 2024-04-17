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

        public void RaiseTick(IStopwatch initiator, string name, int round)
        {
            Console.WriteLine($"{Name} -> {round}, time: {ElapsedTime}, initiator: {initiator}, name: {name}");
        }

        public TimeSpan ElapsedTime { get; set; }
    }
}
