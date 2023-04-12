using Infrastructure.Logger;

namespace Infrastructure.IOManager
{
    public interface IIOManagerFactory
    {
        IIOManager CreateIOManager(ILogger logger);
    }
}
