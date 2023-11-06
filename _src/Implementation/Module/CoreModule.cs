using System;
using System.IO;
using System.Threading;
using Implementation.Application;
using Implementation.Configuration.Factories;
using Implementation.IO;
using Implementation.Navigator.Factories;
using Implementation.Repositories.Factories;
using Implementation.Time;
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
    public class CoreModule : AModule, IGeneralModule, ICancellableModule
    {
        public CancellationTokenSource Source { get; }

        public CoreModule(IServiceCollection serviceCollection, CancellationTokenSource source) : base(serviceCollection)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }
        
        public void RegisterServices(string projectNamespace)
        {
            var mainFolder = Path.Join("joshika39", projectNamespace);
            var userFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), mainFolder);
            Collection.AddTransient<IApplicationSettings, GeneralApplicationSettings>(provider =>
                new GeneralApplicationSettings(userFolder, provider.GetRequiredService<IConfigurationQueryFactory>())
            );

            RegisterServices();
        }

        public override IModule RegisterServices(IServiceCollection collection)
        {
            collection.AddScoped<ILogger, Logger.Logger>(_ => new Logger.Logger(Guid.NewGuid()));
            collection.AddTransient<INavigatorFactory, NavigatorFactory>();
            collection.AddTransient<INavigatorElementFactory, NavigatorElementFactory>();
            collection.AddTransient<IConfigurationQueryFactory, JsonConfigurationQueryFactory>();
            collection.AddScoped<IWriter, Writer>();
            collection.AddScoped<IReader, Reader>();
            collection.AddScoped<IDataParser, DefaultDataParser>();
            collection.AddTransient<IRepositoryFactory, RepositoryFactory>();

            collection.AddScoped<ILifeCycleManager>(_ => new LifeCycleManager(Source));
            collection.AddSingleton<IStopWatchFactory, StopwatchFactory>();
            collection.AddScoped<IStopwatch>(_ => new DefaultStopwatch(Source.Token));
            return this;
        }
    }
}
