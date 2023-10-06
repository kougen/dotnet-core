using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Implementation.Module;
using Implementation.Repositories.Factories;
using ImplementationTest.RepositoryTests.Model;
using Infrastructure.Application;
using Infrastructure.Repositories.Factories;
using Microsoft.Extensions.DependencyInjection;
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
            var fileName = $@".\data\users-{id}.json";
            try
            {
                await using var repository = CreateRepositoryFactory(@".\data").CreateJsonRepository<User>($"users-{id}");
                await repository.DisposeAsync();
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
            var fileName = $@".\data\users-{id}.json";
            
            try
            {
                await using var repository = CreateRepositoryFactory(@".\data").CreateJsonRepository<User>($"users-{id}");
                var user = new User
                {
                    Name = "Peter",
                    Age = 25
                };
                await repository.Create(user).SaveChanges();
                var jsonString = $"[{{\"Id\":\"{user.Id}\",\"Name\":\"{user.Name}\",\"Age\":{user.Age}}}]";

                Assert.True(File.Exists(fileName));
                Assert.Equal(jsonString, await File.ReadAllTextAsync(fileName));
            }
            finally
            {
                if (File.Exists($@".\data\users-{id}.json"))
                {
                    File.Delete(fileName);
                }
            }
        }

        [Fact]
        public async Task JRT_0021_Given_JsonFile_When_GetAllEntitiesCalled_Then_AllEntitiesReturns()
        {
            var fileName = @".\Resources\JRT\0021-users";
            await using var repository = CreateRepositoryFactory(@".\Resources\JRT").CreateJsonRepository<User>(fileName);
            var allUsers = await repository.GetAllEntities();
            Assert.True(File.Exists(fileName));
            Assert.Equal(4, allUsers.Count());
        }

        private IApplicationSettings CreateMockApplicationSettings(string folder)
        {
            var mock = new Mock<IApplicationSettings>();
            mock.Setup((a) => a.ConfigurationFolder).Returns(folder);
            return mock.Object;
        }

        private IRepositoryFactory CreateRepositoryFactory(string folder)
        {
            var applicationSettings = CreateMockApplicationSettings(folder);
            return new RepositoryFactory(applicationSettings);
        }
    }
}
