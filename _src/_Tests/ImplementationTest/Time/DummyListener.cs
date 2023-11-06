using System;
using Infrastructure.Time.Listeners;

namespace ImplementationTest.Time
{
    internal class DummyListener : ITickListener
    {
        public void RaiseTick(int round)
        {
            Console.WriteLine($"Round {round} elapsed time: {ElapsedTime}");
        }

        public TimeSpan ElapsedTime { get; set; }
    }
}
