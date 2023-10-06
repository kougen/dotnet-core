using Infrastructure.Application;
using Infrastructure.Repositories;

namespace Implementation.Repositories
{
    internal class DefaultJsonRepository<T> : AJsonRepository<T> where T: IEntity
    {
        public DefaultJsonRepository(IApplicationSettings applicationSettings, string repositoryKey) : base(applicationSettings, repositoryKey)
        { }
    }
}
