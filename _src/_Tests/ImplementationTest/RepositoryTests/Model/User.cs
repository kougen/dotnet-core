using System;
using Infrastructure.Repositories;

namespace ImplementationTest.RepositoryTests.Model
{
    public class User : IEntity
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
