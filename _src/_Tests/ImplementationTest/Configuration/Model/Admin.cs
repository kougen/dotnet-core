using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ImplementationTest.Configuration.Model
{
    public class Admin
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("role")]
        public string Role { get; set;}
    }
}
