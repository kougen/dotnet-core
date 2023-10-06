namespace Infrastructure.Repositories.Factories
{
    public interface IRepositoryFactory
    {
        IRepository<T> CreateJsonRepository<T>(string repositoryName) where T: IEntity;
    }
}
