﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Implementation.Converters;
using Infrastructure.Application;
using Infrastructure.Repositories;
using Newtonsoft.Json;

namespace Implementation.Repositories
{
    public abstract class AJsonRepository<TInterface, TClass> : IRepository<TInterface> 
        where TInterface : class, IEntity
        where TClass : class, TInterface
    {
        private readonly string _dataPath;
        private readonly string _filePath;
        private readonly IList<TInterface> _updatedEntities;
        private readonly IList<TInterface> _addedEntities;
        private readonly IList<Guid> _removedEntities;
        private bool _isLocked;
        private readonly JsonSerializerSettings _settings;

        protected AJsonRepository(IApplicationSettings applicationSettings, string repositoryKey)
        {
            applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
            repositoryKey = repositoryKey ?? throw new ArgumentNullException(nameof(repositoryKey));

            _updatedEntities = new List<TInterface>();
            _addedEntities = new List<TInterface>();
            _removedEntities = new List<Guid>();
            _dataPath = applicationSettings.RepositoryPath!;
            _filePath = Path.Join(_dataPath, $"{repositoryKey}.json");
            _settings = new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>
                {
                    new GenericJsonConverter<TInterface, TClass>()
                }
            };
        }

        public async Task<IEnumerable<TInterface>> GetAllEntitiesAsync()
        {
            return await GetAllContentAsync();
        }
        public IEnumerable<TInterface> GetAllEntities()
        {
            return GetAllContent();
        }
        public async Task<TInterface?> GetEntityAsync(Guid id)
        {
            var allContent = await GetAllContentAsync();
            return allContent.FirstOrDefault(e => e.Id.Equals(id));
            
        }

        public TInterface? GetEntity(Guid id)
        {
            var allContent = GetAllContent();
            return allContent.FirstOrDefault(e => e.Id.Equals(id));
        }
        public IRepository<TInterface> Create(TInterface entity)
        {
            _addedEntities.Add(entity);
            return this;
        }
        public IRepository<TInterface> Delete(Guid id)
        {
            _removedEntities.Add(id);
            return this;
        }
        public IRepository<TInterface> Update(TInterface entity)
        {
            _updatedEntities.Add(entity);
            return this;
        }
       
        public IRepository<TInterface> SaveChanges()
        {
            if (_isLocked)
            {
                return this;
            }
            
            CreateRepository();
            _isLocked = true;
            
            var newContent = JsonConvert.SerializeObject(PerformSave(GetAllContent().ToList()), _settings);
            File.WriteAllText(_filePath, newContent);
            _isLocked = false;
            return this;
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

        private async Task<IEnumerable<TInterface>> GetAllContentAsync()
        {
            await SaveChangesAsync();
            var allContent = await File.ReadAllTextAsync(_filePath);
            var list = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<TInterface>>(allContent, _settings));
            return list ?? new List<TInterface>();
        }
        
        private IEnumerable<TInterface> GetAllContent()
        {
            SaveChanges();
            var allContent = File.ReadAllText(_filePath);
            var list = JsonConvert.DeserializeObject<IEnumerable<TInterface>>(allContent, _settings);
            return list ?? new List<TInterface>();
        }

        public async Task<IRepository<TInterface>> SaveChangesAsync()
        {
            if (_isLocked)
            {
                return this;
            }
            await CreateRepositoryAsync();
            _isLocked = true;
            var currentContent = PerformSave((await GetAllContentAsync()).ToList());
            var newContent = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(currentContent, _settings));
            await File.WriteAllTextAsync(_filePath, newContent);
            _isLocked = false;
            return await Task.FromResult<IRepository<TInterface>>(this);
        }

        private ICollection<TInterface> PerformSave(ICollection<TInterface> currentContent)
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