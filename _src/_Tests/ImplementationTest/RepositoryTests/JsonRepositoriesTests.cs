﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Implementation.Repositories.Factories;
using ImplementationTest.RepositoryTests.Model;
using Infrastructure.Application;
using Infrastructure.Repositories.Factories;
using Moq;
using Xunit;

namespace ImplementationTest.RepositoryTests
{
    public class JsonRepositoriesTests
    {

        [Fact]
        public async Task JRT_0001_Given_InExistentJsonRepository_When_ConstructorCalled_Then_EmptyRepoIsCreated()
        {
            var id = Guid.NewGuid();
            var fileName = Path.Join(".", "data", $"users-{id}.json");
            try
            {
                await using var repository = CreateRepositoryFactory(Path.Join(".", "data")).CreateRepository<User>($"users-{id}");
                await repository.SaveChangesAsync();
                Assert.True(File.Exists(fileName));
                var text = await File.ReadAllTextAsync(fileName);
                Assert.Equal("[]", text);
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }

        [Fact]
        public async Task JRT_0011_Given_EmptyRepository_When_CreateEntityCalled_Then_EntityCreated()
        {
            var id = Guid.NewGuid();
            var fileName = Path.Join(".", "data", $"users-{id}.json");
            
            try
            {
                await using var repository = CreateRepositoryFactory(Path.Join(".", "data")).CreateRepository<User>($"users-{id}");
                var user = new User
                {
                    Name = "Peter",
                    Age = 25
                };
                await repository.Create(user).SaveChangesAsync();
                var jsonString = $"[{{\"Name\":\"{user.Name}\",\"Age\":{user.Age},\"Id\":\"{user.Id}\"}}]";
                
                Assert.True(File.Exists(fileName));
                Assert.Equal(jsonString, await File.ReadAllTextAsync(fileName));
            }
            finally
            {
                if (File.Exists(Path.Join(".", "data", $"users-{id}.json")))
                {
                    File.Delete(fileName);
                }
            }
        }
        
        [Fact]
        public void JRT_0012_Given_EmptyRepository_When_CreateEntityCalledSynchronously_Then_EntityCreated()
        {
            var id = Guid.NewGuid();
            var fileName = Path.Join(".", "data", $"users-{id}.json");
            
            try
            {
                using var repository = CreateRepositoryFactory(Path.Join(".", "data")).CreateRepository<User>($"users-{id}");
                var user = new User()
                {
                    Name = "Peter",
                    Age = 25
                };
                repository.Create(user);
                repository.SaveChanges();
                var jsonString = $"[{{\"Name\":\"{user.Name}\",\"Age\":{user.Age},\"Id\":\"{user.Id}\"}}]";
                var text = File.ReadAllText(fileName);
                Assert.True(File.Exists(fileName));
                Assert.Equal(jsonString, text);
            }
            finally
            {
                if (File.Exists(Path.Join(".", "data", $"users-{id}.json")))
                {
                    File.Delete(fileName);
                }
            }
        }

        [Fact]
        public void JRT_0021_Given_JsonFile_When_GetAllEntitiesCalled_Then_AllEntitiesReturns()
        {
            var fileName = Path.Join(".", "Resources", "JRT", "0021-users.json");
            using var repository = CreateRepositoryFactory(Path.Join(".", "Resources", "JRT")).CreateRepository<User>("0021-users");
            var allUsers = repository.GetAllEntities().ToList();
            var guid = Guid.Parse("061e971d-5ca2-4b9f-998f-66766b06c4ce");
            Assert.True(File.Exists(fileName));
            Assert.Equal(4, allUsers.Count);
            Assert.Equal(allUsers[0].Id, guid);
        }
        
        [Fact]
        public async Task JRT_0022_Given_JsonFile_When_GetAllEntitiesAsyncCalled_Then_AllEntitiesReturns()
        {
            var fileName = Path.Join(".", "Resources", "JRT", "0021-users.json");
            await using var repository = CreateRepositoryFactory(Path.Join(".", "Resources", "JRT")).CreateRepository<User>("0021-users");
            var allUsers = (await repository.GetAllEntitiesAsync()).ToList();
            var guid = Guid.Parse("061e971d-5ca2-4b9f-998f-66766b06c4ce");
            Assert.True(File.Exists(fileName));
            Assert.Equal(4, allUsers.Count);
            Assert.Equal(allUsers[0].Id, guid);
        }
        
        [Fact]
        public void JRT_0031_Given_JsonFile_When_DeleteFirstFromRepository_Then_EntityIsDeleted()
        {
            var id = Guid.NewGuid();
            var fileName = Path.Join(".", "Resources", "JRT", "0021-users.json");
            var tempFileName = Path.Join(".", "Resources", "JRT", $"{id}.json");
            File.Copy(fileName, tempFileName);
            using var repository = CreateRepositoryFactory(Path.Join(".", "Resources", "JRT")).CreateRepository<User>(id.ToString());
            var fistUser = repository.GetAllEntities().First();
            repository.Delete(fistUser.Id).SaveChanges();
            Assert.True(File.Exists(tempFileName));
            var newCollection = repository.GetAllEntities().ToList();
            Assert.Equal(3, newCollection.Count);
            var guid = Guid.Parse("b4b19e46-b845-4181-9ebe-ed7f5eafbd0d");
            Assert.Equal(newCollection[0].Id, guid);
            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
            Assert.False(File.Exists(tempFileName));
        }

        private IApplicationSettings CreateMockApplicationSettings(string folder)
        {
            var mock = new Mock<IApplicationSettings>();
            mock.Setup((a) => a.ConfigurationFolder).Returns(folder);
            mock.Setup((a) => a.RepositoryPath).Returns(folder);
            return mock.Object;
        }

        private IRepositoryFactory CreateRepositoryFactory(string folder)
        {
            var applicationSettings = CreateMockApplicationSettings(folder);
            return new RepositoryFactory(applicationSettings);
        }
    }
}
