namespace Infrastructure.Repositories.Factories
{
    public interface IRepositoryFactory
    {
        IRepository<T> CreateRepository<T>(string repositoryName) where T : class, IEntity;
    }
}
