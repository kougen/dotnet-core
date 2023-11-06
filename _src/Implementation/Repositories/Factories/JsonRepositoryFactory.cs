using System;
using Infrastructure.Application;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Factories;

namespace Implementation.Repositories.Factories
{
    internal class RepositoryFactory : IRepositoryFactory
    {
        private readonly IApplicationSettings _applicationSettings;
        
        public RepositoryFactory(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
        }
        
        public IRepository<T> CreateRepository<T>(string repositoryName) where T : class, IEntity
        {
            return new DefaultJsonRepository<T>(_applicationSettings, repositoryName);
        }
    }
}
