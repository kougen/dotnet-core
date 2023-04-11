using System;
using System.Reflection;
using Implementation.StandardIOManager;
using Infrastructure.IO;
using Infrastructure.IOManager;
using Infrastructure.Logger;
using Infrastructure.Navigator;
using Unity;
using Unity.RegistrationByConvention;
using Unity.Resolution;

namespace Implementation.Core
{
    public static class Bootsrapper
    {
        public static IUnityContainer GetDefaultContainer(string logPath, Guid id)
        {
            var container = new UnityContainer();

            container.RegisterTypes(
                    AllClasses.FromAssemblies(Assembly.Load("Implementation")),
                    WithMappings.FromMatchingInterface,
                    WithName.Default)
                .RegisterTools(logPath, id);                ;

            return container;
        }
        
        private static IUnityContainer RegisterTools(this IUnityContainer container, string logPath, Guid id)
        {
            container.RegisterFactory<ILogger>(c =>
            {
                return c.Resolve<Logger.Logger>(new ParameterOverride("logPath", logPath),
                    new ParameterOverride("id", id));
            });
            container.RegisterType<IWriter, Writer>();
            container.RegisterType<IReader, Reader>();
            container.RegisterType<INavigator, Navigator.Navigator>();
            return container;
        }
    }
}
