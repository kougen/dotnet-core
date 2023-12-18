using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Time;
using Infrastructure.Time.Listeners;

namespace Implementation.Time
{
    internal class PeriodicStopwatch : IPeriodicStopwatch
    {
        private readonly Stopwatch _stopwatch;
        private readonly IStopwatch _parent;
        private int _periodInMilliseconds;
        private readonly CancellationToken _cancellationToken;
        private int _round;
        private readonly ICollection<ITickListener> _listeners;

        public bool IsRunning => _parent.IsRunning;
        
        public TimeSpan Elapsed { get; private set; }

        public PeriodicStopwatch(IStopwatch parent, int periodInMilliseconds, ITickListener listener, CancellationToken cancellationToken)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _periodInMilliseconds = periodInMilliseconds;
            _cancellationToken = cancellationToken;
            _stopwatch = new Stopwatch();
            _listeners = new List<ITickListener>();
            _listeners.Add(listener);
        }
        
        public PeriodicStopwatch(IStopwatch parent, int periodInMilliseconds, CancellationToken cancellationToken)
        {
            _periodInMilliseconds = periodInMilliseconds;
            _cancellationToken = cancellationToken;
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _stopwatch = new Stopwatch();
            _listeners = new List<ITickListener>();
        }

        public void Start()
        {
            if (!_parent.IsRunning)
            {
                throw new InvalidOperationException("Parent stopwatch is not running");
            }
            
            _stopwatch.Start();
            Task.Run(() =>
            {
                while (!_cancellationToken.IsCancellationRequested && _stopwatch.IsRunning)
                {
                    if (_stopwatch.Elapsed.TotalMilliseconds > _periodInMilliseconds)
                    {
                        foreach (var listener in _listeners)
                        {
                            listener.RaiseTick(_round);
                            listener.ElapsedTime = Elapsed;
                        }
                        Elapsed += _stopwatch.Elapsed;
                        _stopwatch.Restart();
                        _round++;
                    }
                }
            }, _cancellationToken);
        }

        public void ChangePeriod(int periodInMilliseconds)
        {
            _periodInMilliseconds = periodInMilliseconds;
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public void Resume()
        {
            if (!_stopwatch.IsRunning)
            {
                Start();
            }
        }

        public void Reset()
        {
            _stopwatch.Reset();
            _round = 0;
            Elapsed = TimeSpan.Zero;
        }

        public void AttachListener(ITickListener listener)
        {
            _listeners.Add(listener);
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _stopwatch.Reset();
            _listeners.Clear();
            _parent.UnregisterStopwatch(this);
            GC.SuppressFinalize(this);
        }
    }
}
