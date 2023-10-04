using Implementation.Repositories;
using ImplementationTest.RepositoryTests.Model;

namespace ImplementationTest.RepositoryTests
{
    public class UserRepository : AJsonRepository<User>
    {
        public UserRepository(string filePath) : base(filePath)
        { }
        public UserRepository(string directory, string repositoryKey) : base(directory, repositoryKey)
        { }
    }
}
