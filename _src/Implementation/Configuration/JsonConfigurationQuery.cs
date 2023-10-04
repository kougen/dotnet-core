using System;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Implementation.Configuration
{
    internal class JsonConfigurationQuery : IConfigurationQuery
    {
        private readonly string _filePath;

        public JsonConfigurationQuery(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        #region Get
        public async Task<string?> GetStringAttributeAsync(string path)
        {
            return (await GetAttributeAsync(path)).Value<string>();
        }

        public async Task<int> GetIntAttributeAsync(string path)
        {
            return (await GetAttributeAsync(path)).Value<int>();
        }

        public async Task<bool> GetBoolAttributeAsync(string path)
        {
            return (await GetAttributeAsync(path)).Value<bool>();
        }

        public async Task<T?> GetObjectAsync<T>(string path)
        {
            var token = await GetAttributeAsync(path);
            return JsonConvert.DeserializeObject<T>(token?.ToString());
        }
        #endregion

        public async Task SetAttributeAsync(string path, string value)
        {
            await SetObjectAsync(path, value);
        }

        public async Task SetAttributeAsync(string path, int value)
        {
            await SetObjectAsync(path, value);
        }

        public async Task SetAttributeAsync(string path, bool value)
        {
            await SetObjectAsync(path, value);
        }
        
        public async Task SetObjectAsync<T>(string path, T value)
        {
            var jsonObject = JObject.Parse(await File.ReadAllTextAsync(_filePath));
            JToken? token = jsonObject;
            var split = path.Trim('.').Split('.');
            for (var index = 0; index < split.Length; index++)
            {
                var segment = split[index];
                if (token is not JObject jObject)
                {
                    break;
                }
                if (index == split.Length - 1)
                {
                    jObject[segment] = JToken.FromObject(value);
                    break;
                }
                token = jObject.TryGetValue(segment, out var nextToken) ? nextToken : null;
                if (token == null)
                {
                    break;
                }
            }
            await File.WriteAllTextAsync(_filePath, jsonObject.ToString(Formatting.Indented));
        }
        
        private async Task<JToken?> GetAttributeAsync(string path)
        {
            JToken? token = JObject.Parse(await File.ReadAllTextAsync(_filePath));
            foreach (var segment in path.Trim('.').Split('.'))
            {
                if (token is not JObject jObject)
                {
                    return default;
                }

                token = jObject.TryGetValue(segment, out var nextToken) ? nextToken : null;
            }
            return token;
        }
    }
}
