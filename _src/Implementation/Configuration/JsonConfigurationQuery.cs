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
            var result = await GetAttributeAsync(path);
            return result?.Value<string>();
        }

        public async Task<int?> GetIntAttributeAsync(string path)
        {
            var result = await GetAttributeAsync(path);
            return result?.Value<int>();
        }

        public async Task<bool?> GetBoolAttributeAsync(string path)
        {
            var result = await GetAttributeAsync(path);
            return result?.Value<bool>();
        }

        public async Task<T?> GetObjectAsync<T>(string path)
        {
            var token = await GetAttributeAsync(path);
            return token == null ? default : JsonConvert.DeserializeObject<T>(token?.ToString());
        }
        
        public string? GetStringAttribute(string path)
        {
            return GetStringAttributeAsync(path).Result;
        }
        
        public int? GetIntAttribute(string path)
        {
            return GetIntAttributeAsync(path).Result;
        }
        public bool? GetBoolAttribute(string path)
        {
            return GetBoolAttributeAsync(path).Result;
        }
        public T? GetObject<T>(string path)
        {
            return GetObjectAsync<T>(path).Result;
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
            var content = await File.ReadAllTextAsync(_filePath);
            var jsonObject = SetJsonObject(JObject.Parse(content), path, value);
            await File.WriteAllTextAsync(_filePath, jsonObject.ToString(Formatting.Indented));
        }
        public void SetAttribute(string path, string value)
        {
            SetObject(path, value);
        }
        public void SetAttribute(string path, int value)
        {
            SetObject(path, value);
        }
        public void SetAttribute(string path, bool value)
        {
            SetObject(path, value);
        }
        
        public void SetObject<T>(string path, T value)
        {
            var content = File.ReadAllText(_filePath);
            var jsonObject = SetJsonObject(JObject.Parse(content), path, value);
            File.WriteAllText(_filePath, jsonObject.ToString(Formatting.Indented));
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

        private JObject SetJsonObject<T>(JObject jsonObject, string path, T value)
        {
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
                    jObject[segment] = JToken.Parse("{}");
                    token = jObject.TryGetValue(segment, out nextToken) ? nextToken : null;
                }
            }
            
            return jsonObject;
        }
    }
}
