using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public interface IConfigurationQuery
    {
        Task<string?> GetStringAttributeAsync(string path);
        Task<int?> GetIntAttributeAsync(string path);
        Task<bool?> GetBoolAttributeAsync(string path);
        Task<T?> GetObjectAsync<T>(string path);
        string? GetStringAttribute(string path);
        int? GetIntAttribute(string path);
        bool? GetBoolAttribute(string path);
        T? GetObject<T>(string path);
        
        Task SetAttributeAsync(string path, string value);
        Task SetAttributeAsync(string path, int value);
        Task SetAttributeAsync(string path, bool value);
        Task SetObjectAsync<T>(string path, T value);
        void SetAttribute(string path, string value);
        void SetAttribute(string path, int value);
        void SetAttribute(string path, bool value);
        void SetObject<T>(string path, T value);
    }
}
