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
    /// <summary>
    /// Abstract Json implementation of the <see cref="IRepository{T}"/>.
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    public abstract class AJsonRepository<TClass> : IRepository<TClass> where TClass : class, IEntity
    {
        private readonly string _dataPath;
        private readonly string _filePath;
        private readonly IList<TClass> _updatedEntities;
        private readonly IList<TClass> _addedEntities;
        private readonly IList<Guid> _removedEntities;
        private bool _isLocked;

        protected AJsonRepository(IApplicationSettings applicationSettings, string repositoryKey)
        {
            applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
            repositoryKey = repositoryKey ?? throw new ArgumentNullException(nameof(repositoryKey));

            _updatedEntities = new List<TClass>();
            _addedEntities = new List<TClass>();
            _removedEntities = new List<Guid>();
            _dataPath = applicationSettings.RepositoryPath!;
            _filePath = Path.Join(_dataPath, $"{repositoryKey}.json");
        }

        public async Task<IEnumerable<TClass>> GetAllEntitiesAsync()
        {
            return await GetAllContentAsync();
        }
        public IEnumerable<TClass> GetAllEntities()
        {
            return GetAllContent();
        }
        public async Task<TClass?> GetEntityAsync(Guid id)
        {
            var allContent = await GetAllContentAsync();
            return allContent.FirstOrDefault(e => e.Id.Equals(id));
            
        }

        public TClass? GetEntity(Guid id)
        {
            var allContent = GetAllContent();
            return allContent.FirstOrDefault(e => e.Id.Equals(id));
        }
        public IRepository<TClass> Create(TClass entity)
        {
            _addedEntities.Add(entity);
            return this;
        }
        public IRepository<TClass> Delete(Guid id)
        {
            _removedEntities.Add(id);
            return this;
        }
        public IRepository<TClass> Update(TClass entity)
        {
            _updatedEntities.Add(entity);
            return this;
        }
       
        public void SaveChanges()
        {
            if (_isLocked)
            {
                return;
            }
            
            CreateRepository();
            _isLocked = true;
            
            var newContent = JsonConvert.SerializeObject(PerformSave(GetAllContent().ToList()));
            File.WriteAllText(_filePath, newContent);
            _isLocked = false;
        }

        private async Task CreateRepositoryAsync()
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
        
        private void CreateRepository()
        {
            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
                File.AppendAllText(_filePath, "[]");
            }
        }

        private async Task<IEnumerable<TClass>> GetAllContentAsync()
        {
            await SaveChangesAsync();
            var allContent = await File.ReadAllTextAsync(_filePath);
            var list = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<TClass>>(allContent));
            return list ?? new List<TClass>();
        }
        
        private IEnumerable<TClass> GetAllContent()
        {
            SaveChanges();
            var allContent = File.ReadAllText(_filePath);
            var list = JsonConvert.DeserializeObject<IEnumerable<TClass>>(allContent);
            return list ?? new List<TClass>();
        }

        public async Task SaveChangesAsync()
        {
            if (_isLocked)
            {
                return;
            }
            await CreateRepositoryAsync();
            _isLocked = true;
            var currentContent = PerformSave((await GetAllContentAsync()).ToList());
            var newContent = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(currentContent));
            await File.WriteAllTextAsync(_filePath, newContent);
            _isLocked = false;
        }

        private ICollection<TClass> PerformSave(ICollection<TClass> currentContent)
        {
            for (var index = 0; index < _addedEntities.Count; index++)
            {
                var addedEntity = _addedEntities[index];
                if (currentContent.Any(e => e.Id.Equals(addedEntity.Id)))
                {
                    throw new InvalidOperationException("Entity to add already exists in the repository");
                }

                currentContent.Add(addedEntity);
                _addedEntities.Remove(addedEntity);
            }

            for (var index = 0; index < _updatedEntities.Count; index++)
            {
                var updatedEntity = _updatedEntities[index];
                var target = currentContent.FirstOrDefault(e => e.Id.Equals(updatedEntity.Id));

                if (target == null)
                {
                    throw new InvalidOperationException("Entity to update not found in the repository");
                }

                currentContent.Remove(target);
                currentContent.Add(updatedEntity);
                _updatedEntities.Remove(updatedEntity);
            }
            
            for (var index = 0; index < _removedEntities.Count; index++)
            {
                var id = _removedEntities[index];
                currentContent.Remove(currentContent.First(e => e.Id.Equals(id)));
                
                _removedEntities.Remove(id);
            }

            return currentContent;
        }
        
        public async ValueTask DisposeAsync()
        {
            await SaveChangesAsync();
        }

        public void Dispose()
        {
            SaveChanges();
        }
    }
}
