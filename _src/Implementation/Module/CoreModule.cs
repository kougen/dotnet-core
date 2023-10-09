using System;
using System.IO;
using Implementation.Application;
using Implementation.Configuration.Factories;
using Implementation.IO;
using Implementation.IO.Factories;
using Implementation.Navigator.Factories;
using Implementation.Repositories.Factories;
using Infrastructure.Application;
using Infrastructure.Configuration.Factories;
using Infrastructure.IO;
using Infrastructure.IO.Factories;
using Infrastructure.Logger;
using Infrastructure.Module;
using Infrastructure.Navigator.Factories;
using Infrastructure.Repositories.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Implementation.Module
{
    public class CoreModule : IGeneralModule
    {
        public void LoadModules(IServiceCollection collection, string projectNamespace)
        {
            var mainFolder = Path.Join("joshika39", projectNamespace);
            var userFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), mainFolder);
            collection.AddScoped<ILogger, Logger.Logger>(_ => new Logger.Logger(Guid.NewGuid()));
            collection.AddTransient<IIOFactory, IOFactory>();
            collection.AddTransient<INavigatorFactory, NavigatorFactory>();
            collection.AddTransient<INavigatorElementFactory, NavigatorElementFactory>();
            collection.AddTransient<IConfigurationQueryFactory, JsonConfigurationQueryFactory>();
            collection.AddScoped<IWriter, Writer>();
            collection.AddScoped<IReader, Reader>();
            collection.AddTransient<IRepositoryFactory, RepositoryFactory>();
            collection.AddTransient<IApplicationSettings, GeneralApplicationSettings>(provider => 
                new GeneralApplicationSettings(userFolder, provider.GetRequiredService<IConfigurationQueryFactory>())
            );
        }
    }
}
