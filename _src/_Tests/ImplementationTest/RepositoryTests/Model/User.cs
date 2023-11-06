using Implementation.Repositories;

namespace ImplementationTest.RepositoryTests.Model
{
    public class User : AEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
