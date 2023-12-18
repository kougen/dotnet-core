using System;
using System.Threading;
using Infrastructure.Time;
using Infrastructure.Time.Factories;

namespace Implementation.Time.Factories;

public class PeriodicStopwatchFactory : IPeriodicStopwatchFactory
{
    private readonly IStopwatch _parent;
    private readonly CancellationToken _token;

    public PeriodicStopwatchFactory(IStopwatch parent, CancellationToken token)
    {
        _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        _token = token;
    }
    
    public IPeriodicStopwatch CreatePeriodicStopwatch(int periodInMilliseconds)
    {
        var periodicStopwatch = new PeriodicStopwatch(_parent, periodInMilliseconds, _token);
        _parent.RegisterStopwatch(periodicStopwatch);
        return periodicStopwatch;
    }
}