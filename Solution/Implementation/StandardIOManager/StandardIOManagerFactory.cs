using Infrastructure.IOManager;
using Infrastructure.Logger;

namespace Implementation.StandardIOManager
{
    internal class StandardIOManagerFactory : IIOManagerFactory
    {
        public IIOManager CreateIOManager(ILogger logger)
        {
            return new StandardIOManager();
        }
    }
}

