using System;
using System.IO;
using System.Threading;
using Implementation.Application;
using Implementation.Configuration.Factories;
using Implementation.IO;
using Implementation.Navigator.Factories;
using Implementation.Repositories.Factories;
using Implementation.Time.Factories;
using Infrastructure.Application;
using Infrastructure.Configuration.Factories;
using Infrastructure.IO;
using Infrastructure.Logger;
using Infrastructure.Module;
using Infrastructure.Navigator.Factories;
using Infrastructure.Repositories.Factories;
using Infrastructure.Time;
using Infrastructure.Time.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Implementation.Module
{
    public class CoreModule : IGeneralModule, ICancellableModule
    {
        public CancellationTokenSource Source { get; }

        public CoreModule()
        {
            
        }
        
        public void RegisterServices(IServiceCollection collection, string projectNamespace)
        {
            var tokenSource = new CancellationTokenSource();
            var mainFolder = Path.Join("joshika39", projectNamespace);
            var userFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), mainFolder);
            collection.AddScoped<ILogger, Logger.Logger>(_ => new Logger.Logger(Guid.NewGuid()));
            collection.AddTransient<INavigatorFactory, NavigatorFactory>();
            collection.AddTransient<INavigatorElementFactory, NavigatorElementFactory>();
            collection.AddTransient<IConfigurationQueryFactory, JsonConfigurationQueryFactory>();
            collection.AddScoped<IWriter, Writer>();
            collection.AddScoped<IReader, Reader>();
            collection.AddScoped<IDataParser, DefaultDataParser>();
            collection.AddTransient<IRepositoryFactory, RepositoryFactory>();

            collection.AddScoped<ILifeCycleManager>(_ => new LifeCycleManager(tokenSource));
            collection.AddSingleton<IStopWatchFactory, StopwatchFactory>();
            collection.AddScoped<IStopwatch>(_ => new DefaultStopwatch(tokenSource.Token));

            collection.AddTransient<IApplicationSettings, GeneralApplicationSettings>(provider =>
                    new GeneralApplicationSettings(userFolder, provider.GetRequiredService<IConfigurationQueryFactory>())
                );
        }
    }
}
