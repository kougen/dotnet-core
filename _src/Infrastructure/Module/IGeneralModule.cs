using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Module
{
    /// <summary>
    /// Registers the services in the <see cref="IServiceCollection" /> for the module.
    /// </summary>
    public interface IGeneralModule
    {
        /// <summary>
        /// Registers the services.
        /// </summary>
        /// <param name="projectNamespace">The name of the project which uses this module</param>
        void RegisterServices(string projectNamespace);
    }
}
