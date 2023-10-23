using System;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Application;
using Infrastructure.Configuration;
using Infrastructure.Configuration.Factories;

namespace Implementation.Application
{
    internal class GeneralApplicationSettings : IApplicationSettings
    {
        private readonly IConfigurationQuery _configQuery;
        public string ApplicationConfigurationFile { get; }
        public string ConfigurationFolder { get; }
        public string? RepositoryPath => GetRepositoryPath();

        public GeneralApplicationSettings(string configurationFolder, IConfigurationQueryFactory queryFactory)
        {
            queryFactory = queryFactory ?? throw new ArgumentNullException(nameof(queryFactory));
            ConfigurationFolder = configurationFolder ?? throw new ArgumentNullException(nameof(configurationFolder));
            ApplicationConfigurationFile = Path.Join(ConfigurationFolder, "config.json");
            _configQuery = queryFactory.CreateConfigurationQuery(ApplicationConfigurationFile);
            _configQuery.SetAttribute("general.repositories", Path.Join(ConfigurationFolder, "Repositories"));
        }

        private string? GetRepositoryPath()
        {
            return _configQuery.GetStringAttribute("general.repositories");
        }
    }
}
