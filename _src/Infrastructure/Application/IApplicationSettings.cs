using System.Threading.Tasks;

namespace Infrastructure.Application
{
    public interface IApplicationSettings
    {
        string ApplicationConfigurationFile { get; }
        string ConfigurationFolder { get; }
        string? RepositoryPath { get; }
    }
}
