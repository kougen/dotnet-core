using System.Threading;

namespace Infrastructure.Time.Factories;

public interface IPeriodicStopwatchFactory
{
    IPeriodicStopwatch CreatePeriodicStopwatch(int periodInMilliseconds);
}