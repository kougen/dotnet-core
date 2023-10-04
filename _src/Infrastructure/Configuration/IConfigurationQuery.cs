using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public interface IConfigurationQuery
    {
        Task<string?> GetStringAttributeAsync(string path);
        Task<int> GetIntAttributeAsync(string path);
        Task<bool> GetBoolAttributeAsync(string path);
        Task<T?> GetObjectAsync<T>(string path);
        
        Task SetAttributeAsync(string path, string value);
        Task SetAttributeAsync(string path, int value);
        Task SetAttributeAsync(string path, bool value);
        Task SetObjectAsync<T>(string path, T value);
    }
}
