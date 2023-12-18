using System.IO;
using System.Threading.Tasks;
using Implementation.Configuration;
using ImplementationTest.Configuration.Model;
using Xunit;

namespace ImplementationTest.Configuration
{
    public class JsonConfigurationQueryTests
    {
        [Fact]
        public async Task JCQT_0001()
        {
            var filePath = Path.Join(".", "Resources", "JCQT", "0001.json");

            var jsonService = new JsonConfigurationQuery(filePath);

            var test = await jsonService.GetStringAttributeAsync("server.db.connection");
        }

        [Fact]
        public async Task JCQT_0002()
        {
            var filePath = Path.Join(".", "Resources", "JCQT", "0001.json");

            var jsonService = new JsonConfigurationQuery(filePath);

            await jsonService.SetAttributeAsync("server.db.connection", "changed:connection");
        }
        
        [Fact]
        public async Task JCQT_0003()
        {
            var filePath = Path.Join(".", "Resources", "JCQT", "0001.json");

            var jsonService = new JsonConfigurationQuery(filePath);

            await jsonService.SetAttributeAsync("server.db.port", 1234);
        }
        
        [Fact]
        public async Task JCQT_0004()
        {
            var filePath = Path.Join(".", "Resources", "JCQT", "0001.json");

            var jsonService = new JsonConfigurationQuery(filePath);

            var admin = await jsonService.GetObjectAsync<Admin>("server.admin");
        }
        
        [Fact]
        public async Task JCQT_0005()
        {
            var filePath = Path.Join(".", "Resources", "JCQT", "0001.json");

            var jsonService = new JsonConfigurationQuery(filePath);

            var admin = await jsonService.GetObjectAsync<Admin>("server.admin");
            admin.Name = "Kayaba Akihiko";

            await jsonService.SetObjectAsync("server.admin", admin);
            
            admin = await jsonService.GetObjectAsync<Admin>("server.admin");
            Assert.Equal("Kayaba Akihiko", admin.Name);
        }
    }
}
