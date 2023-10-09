using System;
using Infrastructure.Repositories;

namespace ImplementationTest.RepositoryTests.Model
{
    public interface IUser : IEntity
    {
        string Name { get; set; }
        int Age { get; set; }
    }
}
