using Infrastructure.Application;
using Infrastructure.Repositories;

namespace Implementation.Repositories
{
    internal class DefaultJsonRepository<TInterface, TClass> : AJsonRepository<TInterface, TClass>
        where TInterface: class, IEntity
        where TClass: class, TInterface
    {
        public DefaultJsonRepository(IApplicationSettings applicationSettings, string repositoryKey) : base(applicationSettings, repositoryKey)
        { }
    }
}
