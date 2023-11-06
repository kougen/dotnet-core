using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Time;
using Infrastructure.Time.Listeners;

namespace Implementation.Time
{
    internal sealed class DefaultStopwatch : IStopwatch
    {
        private readonly CancellationToken _cancellationToken;
        private readonly Stopwatch _stopwatch;
        private readonly ICollection<PeriodicStopwatch> _periodicStopwatches;
        private bool _disposed;
        private readonly ICollection<ITickListener> _listeners;
        
        public bool IsRunning => _stopwatch.IsRunning;
        public TimeSpan Elapsed { get; private set; }

        public DefaultStopwatch(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _periodicStopwatches = new List<PeriodicStopwatch>();
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
                        listener.RaiseTick(0);
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
        
        public void PeriodicOperation(int periodInMilliseconds, ITickListener listener, CancellationToken cancellationToken)
        {
            var periodicStopwatch = new PeriodicStopwatch(periodInMilliseconds, listener, cancellationToken);
            _periodicStopwatches.Add(periodicStopwatch);
            periodicStopwatch.Start();
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
