using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Application;
using Infrastructure.Repositories;
using Newtonsoft.Json;

namespace Implementation.Repositories
{
    public abstract class AJsonRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly string _dataPath;
        private readonly string _filePath;
        private readonly IList<T> _updatedEntities;
        private readonly IList<T> _addedEntities;
        private readonly IList<Guid> _removedEntities;
        private bool _isLocked;

        protected AJsonRepository(IApplicationSettings applicationSettings, string repositoryKey)
        {
            applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
            repositoryKey = repositoryKey ?? throw new ArgumentNullException(nameof(repositoryKey));

            _updatedEntities = new List<T>();
            _addedEntities = new List<T>();
            _removedEntities = new List<Guid>();
            Guid.NewGuid();
            _dataPath = Path.Join(applicationSettings.ConfigurationFolder, "Repositories");
            _filePath = Path.Join(_dataPath, $"{repositoryKey}.json");
        }

        public async Task<IEnumerable<T>> GetAllEntities()
        {
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
            await SaveChanges();
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
            new Task(Action).RunSynchronously();
        }

        private async void Action()
        {
            await SaveChanges();
        }
    }
}
