using Infrastructure.Configuration;
using Infrastructure.Configuration.Factories;

namespace Implementation.Configuration.Factories
{
    internal class JsonConfigurationQueryFactory : IConfigurationQueryFactory
    {
        public IConfigurationQuery CreateConfigurationQuery(string filePath)
        {
            return new JsonConfigurationQuery(filePath);
        }
    }
}
