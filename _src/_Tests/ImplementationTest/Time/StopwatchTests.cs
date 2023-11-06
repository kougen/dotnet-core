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
            listenerMock.Verify(l => l.RaiseTick(0), Times.Exactly(10));
        }
    }
}
