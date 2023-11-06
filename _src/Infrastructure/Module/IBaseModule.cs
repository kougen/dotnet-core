using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Module
{
    public interface IBaseModule
    {
        /// <summary>
        /// Shows whether the modules is already registered or not. 
        /// </summary>
        bool IsRegistered { get; }
        
        /// <summary>
        /// Registers the services into the specified collection.
        /// </summary>
        /// <param name="collection">The <see cref="IServiceCollection"/> created in the composition root.</param>
        IModule RegisterServices(IServiceCollection collection);
    }
}
