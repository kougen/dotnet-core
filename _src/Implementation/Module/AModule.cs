using System;
using Infrastructure.Module;
using Microsoft.Extensions.DependencyInjection;

namespace Implementation.Module
{
    /// <inheritdoc />
    public abstract class AModule : IModule
    {
        protected IServiceCollection Collection;
        
        /// <inheritdoc cref="IModule.IsRegistered"/>
        public bool IsRegistered { get; } = false;

        protected AModule(IServiceCollection collection)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public virtual IServiceCollection RegisterServices()
        {
            if (IsRegistered)
            {
                return Collection;
            }

            RegisterServices(Collection);

            return Collection;
        }

        public abstract IModule RegisterServices(IServiceCollection collection);
        
        public virtual IModule RegisterOtherServices(IBaseModule module)
        {
            if (module.IsRegistered)
            {
                return this;
            }

            module.RegisterServices(Collection);
            return this;
        }
    }
}
