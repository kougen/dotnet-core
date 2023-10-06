using System.IO;
using Infrastructure.Configuration;
using Infrastructure.Configuration.Factories;

namespace Implementation.Configuration.Factories
{
    internal class JsonConfigurationQueryFactory : IConfigurationQueryFactory
    {
        public IConfigurationQuery CreateConfigurationQuery(string filePath)
        {
            var folder = Path.GetDirectoryName(filePath) ?? "./";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                File.WriteAllText(filePath, "{}");
            }
            
            return new JsonConfigurationQuery(filePath);
        }
    }
}
