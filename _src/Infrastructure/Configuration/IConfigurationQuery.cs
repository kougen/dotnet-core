using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    /// <summary>
    /// Handles storing data in the implemented source
    /// </summary>
    public interface IConfigurationQuery
    {
        /// <summary>
        /// Asynchronously retrieves a string attribute from the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <returns>An optional string value or null if not found.</returns>
        Task<string?> GetStringAttributeAsync(string path);

        /// <summary>
        /// Asynchronously retrieves an integer attribute from the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <returns>An optional integer value or null if not found.</returns>
        Task<int?> GetIntAttributeAsync(string path);

        /// <summary>
        /// Asynchronously retrieves a boolean attribute from the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <returns>An optional boolean value or null if not found.</returns>
        Task<bool?> GetBoolAttributeAsync(string path);

        /// <summary>
        /// Asynchronously retrieves an object of type T from the configuration.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="path">The path to the object.</param>
        /// <returns>An optional object of type T or null if not found.</returns>
        Task<T?> GetObjectAsync<T>(string path);

        /// <summary>
        /// Retrieves a string attribute from the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <returns>An optional string value or null if not found.</returns>
        string? GetStringAttribute(string path);

        /// <summary>
        /// Retrieves an integer attribute from the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <returns>An optional integer value or null if not found.</returns>
        int? GetIntAttribute(string path);

        /// <summary>
        /// Retrieves a boolean attribute from the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <returns>An optional boolean value or null if not found.</returns>
        bool? GetBoolAttribute(string path);

        /// <summary>
        /// Retrieves an object of type T from the configuration.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="path">The path to the object.</param>
        /// <returns>An optional object of type T or null if not found.</returns>
        T? GetObject<T>(string path);

        /// <summary>
        /// Asynchronously sets a string attribute in the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <param name="value">The string value to set.</param>
        Task SetAttributeAsync(string path, string value);

        /// <summary>
        /// Asynchronously sets an integer attribute in the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <param name="value">The integer value to set.</param>
        Task SetAttributeAsync(string path, int value);

        /// <summary>
        /// Asynchronously sets a boolean attribute in the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <param name="value">The boolean value to set.</param>
        Task SetAttributeAsync(string path, bool value);

        /// <summary>
        /// Asynchronously sets an object of type T in the configuration.
        /// </summary>
        /// <typeparam name="T">The type of the object to set.</typeparam>
        /// <param name="path">The path to the object.</param>
        /// <param name="value">The object of type T to set.</param>
        Task SetObjectAsync<T>(string path, T value);

        /// <summary>
        /// Sets a string attribute in the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <param name="value">The string value to set.</param>
        void SetAttribute(string path, string value);

        /// <summary>
        /// Sets an integer attribute in the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <param name="value">The integer value to set.</param>
        void SetAttribute(string path, int value);

        /// <summary>
        /// Sets a boolean attribute in the configuration.
        /// </summary>
        /// <param name="path">The path to the attribute.</param>
        /// <param name="value">The boolean value to set.</param>
        void SetAttribute(string path, bool value);

        /// <summary>
        /// Sets an object of type T in the configuration.
        /// </summary>
        /// <typeparam name="T">The type of the object to set.</typeparam>
        /// <param name="path">The path to the object.</param>
        /// <param name="value">The object of type T to set.</param>
        void SetObject<T>(string path, T value);
    }
}
