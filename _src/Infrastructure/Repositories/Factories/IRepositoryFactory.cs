namespace Infrastructure.Repositories.Factories
{
    public interface IRepositoryFactory
    {
        IRepository<TInterface> CreateJsonRepository<TInterface, T>(string repositoryName)
            where TInterface : class, IEntity
            where T : class, TInterface;
    }
}
