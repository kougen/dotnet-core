using System;
using System.Threading;
using Implementation.Module;
using Microsoft.Extensions.DependencyInjection;

namespace ManualTests
{
    internal class Core
    {
        public IServiceProvider LoadModules()
        {
            var collection = new ServiceCollection();
            new CoreModule(collection, new CancellationTokenSource()).RegisterServices("ManualTests");
            return collection.BuildServiceProvider();
        }
    }
}
