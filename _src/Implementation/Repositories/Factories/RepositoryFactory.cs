using System;
using Infrastructure.Application;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Factories;

namespace Implementation.Repositories.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IApplicationSettings _applicationSettings;
        
        public RepositoryFactory(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
        }
        
        public IRepository<TInterface> CreateJsonRepository<TInterface, T>(string repositoryName) 
            where TInterface : class, IEntity
            where T : class, TInterface
        {
            return new DefaultJsonRepository<TInterface, T>(_applicationSettings, repositoryName);
        }
    }
}
