using System.Threading;
using Infrastructure.Time;
using Infrastructure.Time.Factories;

namespace Implementation.Time.Factories
{
    internal class StopwatchFactory : IStopWatchFactory
    {
        public IStopwatch CreateStopwatch(CancellationToken token)
        {
            return new DefaultStopwatch(token);
        }
    }
}
