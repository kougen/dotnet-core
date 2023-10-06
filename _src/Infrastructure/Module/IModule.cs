using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Module
{
    public interface IModule
    {
        void LoadModules(IServiceCollection collection);
    }
}
