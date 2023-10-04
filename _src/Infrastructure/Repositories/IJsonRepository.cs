namespace Infrastructure.Repositories
{
    public interface IJsonRepository<T> : IRepository<T> where T: IEntity
    { }
}
