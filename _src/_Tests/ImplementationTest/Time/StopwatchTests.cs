using System;
using System.Threading;
using System.Threading.Tasks;
using Implementation.Time;
using Infrastructure.Time;
using Infrastructure.Time.Listeners;
using Moq;
using Xunit;

namespace ImplementationTest.Time
{
    public class StopwatchTests
    {
        [Fact]
        public async Task ST_0001_Given_Stopwatch_When_WaitAsyncIsCalled_Then_ListenerGetsNotified()
        {
            var listenerMock = new Mock<ITickListener>();
            var cancellationTokenSource = new CancellationTokenSource();
            using IStopwatch stopwatch = new DefaultStopwatch(cancellationTokenSource.Token);
            Assert.NotNull(stopwatch);
            stopwatch.Start();
            for (var i = 0; i < 10; i++)
            {
                await stopwatch.WaitAsync(250, listenerMock.Object);
            }
            stopwatch.Stop();
            Assert.True(stopwatch.Elapsed.TotalMilliseconds > 2500);
            listenerMock.Verify(l => l.RaiseTick(-1), Times.Exactly(10));
        }

        [Fact]
        public void ST_0011_Given_Stopwatch_When_ManyListeners_Then_NotAffectsPerformance()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            using IStopwatch stopwatch = new DefaultStopwatch(cancellationTokenSource.Token);
            Assert.NotNull(stopwatch);
            
            var pWatchFactory = stopwatch.GetPeriodicStopwatchFactory();
            using var pWatch1 = pWatchFactory.CreatePeriodicStopwatch(900);
            var listenerMock1 = new Mock<ITickListener>();
            pWatch1.AttachListener(listenerMock1.Object);
            stopwatch.Start();
            Thread.Sleep(7000);
            stopwatch.Stop();
            listenerMock1.Verify(l => l.RaiseTick(It.IsAny<int>()), Times.AtLeast(7));
            Assert.False(stopwatch.IsRunning);
            Assert.False(pWatch1.IsRunning);
        }
    }
}
