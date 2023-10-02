using Infrastructure.Logger;

namespace Infrastructure.IO.Factories
{
    public interface IIOFactory
    {
        IReader CreateReader(ILogger logger, IWriter writer);

        IWriter CreateWriter(ILogger logger);
    }
}