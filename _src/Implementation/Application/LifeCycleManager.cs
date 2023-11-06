using System;
using System.Threading;
using Infrastructure.Application;

namespace Implementation.Application
{
    internal class LifeCycleManager : ILifeCycleManager
    {
        public CancellationTokenSource Source { get; }
        public CancellationToken Token => Source.Token;
        
        public LifeCycleManager(CancellationTokenSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }
    }
}
