using System;
using Implementation.Module;
using Microsoft.Extensions.DependencyInjection;

namespace ManualTests
{
    internal class Core
    {
        public IServiceProvider LoadModules()
        {
            var core = new CoreModule();
            var collection = new ServiceCollection();
            
            core.LoadModules(collection);
            
            return collection.BuildServiceProvider();
        }
    }
}
