using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Module
{
    /// <summary>
    /// Registers the services in the <see cref="IServiceCollection" /> for the module.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Registers the services.
        /// </summary>
        /// <returns>Returns self after registered the services</returns>
        IServiceCollection RegisterServices();
        
        /// <summary>
        /// Registers the services of a different module.
        /// </summary>
        /// <param name="module">Other module</param>
        IModule RegisterOtherServices(IBaseModule module);
    }
}
