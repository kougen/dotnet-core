using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Abstract repository interface.
    /// </summary>
    /// <typeparam name="T">Type reference of the data which will be stored in the repository</typeparam>
    public interface IRepository<T> : IAsyncDisposable, IDisposable where T: IEntity
    {
        /// <summary>
        /// Querying the data from the source asynchronously.
        /// </summary>
        /// <returns>List of the stored data.</returns>
        Task<IEnumerable<T>> GetAllEntitiesAsync();
        
        /// <summary>
        /// Querying the data from the source synchronously.
        /// </summary>
        /// <returns>List of the stored data.</returns>
        IEnumerable<T> GetAllEntities();
        
        /// <summary>
        /// Gets one object from the repository asynchronously.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of the object</param>
        /// <returns>The found object or null.</returns>
        Task<T?> GetEntityAsync(Guid id);
        
        /// <summary>
        /// Gets one object from the repository synchronously.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of the object</param>
        /// <returns>The found object or null.</returns>
        T? GetEntity(Guid id);
        
        /// <summary>
        /// Creates a record in the source of the data.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IRepository<T> Create(T entity);
        IRepository<T> Delete(Guid id);
        IRepository<T> Update(T entity);
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
