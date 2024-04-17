using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Implementation.Time.Factories;
using Infrastructure.Time;
using Infrastructure.Time.Factories;
using Infrastructure.Time.Listeners;

namespace Implementation.Time
{
    internal sealed class DefaultStopwatch : IStopwatch
    {
        private readonly CancellationToken _cancellationToken;
        private readonly Stopwatch _stopwatch;
        private readonly ICollection<IPeriodicStopwatch> _periodicStopwatches;
        private bool _disposed;
        private readonly ICollection<ITickListener> _listeners;
        
        public bool IsRunning => _stopwatch.IsRunning;
        public TimeSpan Elapsed { get; private set; }

        public DefaultStopwatch(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _periodicStopwatches = new List<IPeriodicStopwatch>();
            _stopwatch = new Stopwatch();
            _listeners = new List<ITickListener>();
        }
        
        ~DefaultStopwatch()
        {
            Dispose(false);
        }
        
        public void Wait(int periodInMilliseconds, ITickListener listener)
        {
            var startedTime = Elapsed;
            Task.Run(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    if (Elapsed.TotalMilliseconds > startedTime.TotalMilliseconds + periodInMilliseconds)
                    {
                        listener.RaiseTick(0);
                        break;
                    }
                }
            }, _cancellationToken);
        }
        public async Task WaitAsync(int periodInMilliseconds, ITickListener listener)
        {
            await Task.Run(() =>
            {
                var startedTime = Elapsed;
                while (!_cancellationToken.IsCancellationRequested)
                {
                    if (Elapsed.TotalMilliseconds > startedTime.TotalMilliseconds + periodInMilliseconds)
                    {
                        listener.RaiseTick(-1);
                        break;
                    }
                }
            }, _cancellationToken);
        }

        public void Start()
        {
            _stopwatch.Start();
            Task.Run(() =>
            {
                while (!_cancellationToken.IsCancellationRequested && _stopwatch.IsRunning)
                {
                    Elapsed = _stopwatch.Elapsed;
                    foreach (var listener in _listeners)
                    {
                        listener.ElapsedTime = Elapsed;
                    }
                }
            }, _cancellationToken);
            
            foreach (var periodicStopwatch in _periodicStopwatches)
            {
                periodicStopwatch.Resume();
            }
        }
        
        public void Stop()
        {
            _stopwatch.Stop();
            foreach (var periodicStopwatch in _periodicStopwatches)
            {
                periodicStopwatch.Stop();
            }
        }
        
        public void Reset()
        {
            _stopwatch.Reset();
            foreach (var periodicStopwatch in _periodicStopwatches)
            {
                periodicStopwatch.Reset();
            }
        }

        public IPeriodicStopwatch PeriodicOperation(int periodInMilliseconds, ITickListener listener, CancellationToken cancellationToken)
        {
            var periodicStopwatch = new PeriodicStopwatch(this, periodInMilliseconds, listener, cancellationToken);
            _periodicStopwatches.Add(periodicStopwatch);
            periodicStopwatch.Start();
            return periodicStopwatch;
        }
        
        public IPeriodicStopwatch PeriodicOperation(int periodInMilliseconds, ITickListener listener)
        {
            return PeriodicOperation(periodInMilliseconds, listener, _cancellationToken);
        }

        public IPeriodicStopwatch NamedPeriodicOperation(int periodInMilliseconds, ITickListener listener, string name)
        {
            return NamedPeriodicOperation(periodInMilliseconds, listener, name, _cancellationToken);
        }

        public IPeriodicStopwatch NamedPeriodicOperation(int periodInMilliseconds, ITickListener listener, string name,
            CancellationToken cancellationToken)
        {
            var periodicStopwatch = new PeriodicStopwatch(name, this, periodInMilliseconds, listener, cancellationToken);
            _periodicStopwatches.Add(periodicStopwatch);
            periodicStopwatch.Start();
            return periodicStopwatch;
        }

        public IPeriodicStopwatchFactory GetPeriodicStopwatchFactory()
        {
            return new PeriodicStopwatchFactory(this, _cancellationToken);
        }
        
        public void UnregisterStopwatch(IPeriodicStopwatch stopwatch)
        {
            if (_periodicStopwatches.Contains(stopwatch))
            {
                _periodicStopwatches.Remove(stopwatch);
            }
        }

        public void RegisterStopwatch(IPeriodicStopwatch stopwatch)
        {
            if (!_periodicStopwatches.Contains(stopwatch))
            {
                _periodicStopwatches.Add(stopwatch);
            }
        }

        public IPeriodicStopwatch PeriodicOperation(int periodInMilliseconds, CancellationToken cancellationToken)
        {
            var periodicStopwatch = new PeriodicStopwatch(this, periodInMilliseconds, cancellationToken);
            _periodicStopwatches.Add(periodicStopwatch);
            periodicStopwatch.Start();
            return periodicStopwatch;
        }

        public void AttachListener(ITickListener listener)
        {
            _listeners.Add(listener);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            
            if (disposing)
            {
                while (_periodicStopwatches.Any())
                {
                    var stopwatch = _periodicStopwatches.First();
                    _periodicStopwatches.Remove(stopwatch);
                    stopwatch.Dispose();
                }

                while (_listeners.Any())
                {
                    var listener = _listeners.First();
                    _listeners.Remove(listener);
                }
            }

            _disposed = true;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
