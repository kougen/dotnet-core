using System;
using Implementation.Configuration.Factories;
using Implementation.IO;
using Implementation.IO.Factories;
using Implementation.Logger.Factories;
using Implementation.Navigator.Factories;
using Infrastructure.Configuration.Factories;
using Infrastructure.IO;
using Infrastructure.IO.Factories;
using Infrastructure.Logger;
using Infrastructure.Logger.Factories;
using Infrastructure.Module;
using Infrastructure.Navigator.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Implementation.Module
{
    public class CoreModule : IModule
    {

        public void LoadModules(IServiceCollection collection)
        {
            collection.AddScoped<ILogger, Logger.Logger>(_ => new Logger.Logger(Guid.NewGuid()));
            
            collection.AddTransient<IIOFactory, IOFactory>();
            collection.AddTransient<INavigatorFactory, NavigatorFactory>();
            collection.AddTransient<INavigatorElementFactory, NavigatorElementFactory>();
            collection.AddTransient<IConfigurationQueryFactory, JsonConfigurationQueryFactory>();
            collection.AddScoped<IWriter, Writer>();
            collection.AddScoped<IReader, Reader>();
        }
    }
}
