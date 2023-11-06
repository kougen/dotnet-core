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
        /// <param name="collection">The <see cref="IServiceCollection"/> created in the composition root.</param>
        /// <param name="projectNamespace">The name of the project which uses this module</param>
        void RegisterServices(IServiceCollection collection, string projectNamespace);
    }
}
