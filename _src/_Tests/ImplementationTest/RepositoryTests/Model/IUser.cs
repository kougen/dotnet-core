using System;
using Infrastructure.Repositories;

namespace ImplementationTest.RepositoryTests.Model
{
    public interface IUser : IEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
