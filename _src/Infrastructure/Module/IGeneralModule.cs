using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Module
{
    public interface IGeneralModule
    {
        void LoadModules(IServiceCollection collection, string projectNamespace);
    }
}
