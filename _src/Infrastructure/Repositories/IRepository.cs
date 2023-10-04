using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IRepository<T> : IAsyncDisposable, IDisposable where T: IEntity
    {
        Task<IEnumerable<T>> GetAllEntities();
        Task<T?> GetEntity(Guid id);
        IRepository<T> Create(T entity);
        IRepository<T> Delete(Guid id);
        IRepository<T> Update(T entity);
        Task<IRepository<T>> SaveChanges();
    }
}
