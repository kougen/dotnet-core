#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using Newtonsoft.Json;

namespace Implementation.Repositories
{
    internal class JsonRepository<T> : IJsonRepository<T> where T: IEntity
    {
        private readonly string _dataPath;
        private readonly string _filePath;
        private readonly Guid _id;
        private readonly IList<T> _updatedEntities;
        private readonly IList<T> _addedEntities;
        private readonly IList<Guid> _removedEntities;
        private bool _isLocked = false;
        
        public JsonRepository(string filePath)
        {
            _updatedEntities = new List<T>();
            _addedEntities = new List<T>();
            _removedEntities = new List<Guid>();
            
            _id = Guid.NewGuid();
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _dataPath = "./";
        }
        
        public JsonRepository(string directory, string repositoryKey)
        {
            _updatedEntities = new List<T>();
            _addedEntities = new List<T>();
            _removedEntities = new List<Guid>();
            _id = Guid.NewGuid();
            repositoryKey = repositoryKey ?? throw new ArgumentNullException(nameof(repositoryKey));
            _dataPath = directory ?? throw new ArgumentNullException(nameof(directory));
            _filePath = $@"{_dataPath}\{repositoryKey}.json";
        }
        public async Task<IEnumerable<T>> GetAllEntities()
        {
            await SaveChanges();
            return await GetAllContent();
        }
        public async Task<T?> GetEntity(Guid id)
        {
            var allContent = await GetAllContent();
            return allContent.FirstOrDefault(e => e.Id.Equals(id));
        }
        public IRepository<T> Create(T entity)
        {
            _addedEntities.Add(entity);
            return this;
        }
        public IRepository<T> Delete(Guid id)
        {
            _removedEntities.Add(id);
            return this;
        }
        public IRepository<T> Update(T entity)
        {
            _updatedEntities.Add(entity);
            return this;
        }

        private async Task CreateRepository()
        {
            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
                await File.AppendAllTextAsync(_filePath, "[]");
            }
        }

        private async Task<IEnumerable<T>> GetAllContent()
        {
            var allContent = await File.ReadAllTextAsync(_filePath);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<T>>(allContent));
        }

        public async Task<IRepository<T>> SaveChanges()
        {
            if (_isLocked)
            {
                return await Task.FromResult<IRepository<T>>(this);
            }
            await CreateRepository();
            _isLocked = true;
            var currentContent = (await GetAllContent()).ToList();
            currentContent.AddRange(_addedEntities);

            foreach (var updatedEntity in _updatedEntities)
            {
                var target = currentContent.FirstOrDefault(e => e.Id.Equals(updatedEntity.Id));
                if (target != null)
                {
                    currentContent.Remove(target);
                    currentContent.Add(updatedEntity);
                }
            }
            
            foreach (var id in _removedEntities)
            {
                if (currentContent.Any(e => e.Id.Equals(id)))
                {
                    currentContent.Remove(currentContent.First(e => e.Id.Equals(id)));
                }
            }
            
            var newContent = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(currentContent));
            await File.WriteAllTextAsync(_filePath, newContent);
            _isLocked = false;
            return await Task.FromResult<IRepository<T>>(this);
        }
        
        public async ValueTask DisposeAsync()
        {
            await SaveChanges();
        }
        
        public void Dispose()
        {
            new Task(async () => { await SaveChanges(); }).RunSynchronously();
        }
    }
}
