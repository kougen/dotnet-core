using System;
using Implementation.Module;
using Microsoft.Extensions.DependencyInjection;

namespace ManualTests
{
    internal class Core
    {
        public IServiceProvider LoadModules()
        {
            var collection = new ServiceCollection();
            new CoreModule().LoadModules(collection, "ManualTests");
            return collection.BuildServiceProvider();
        }
    }
}
