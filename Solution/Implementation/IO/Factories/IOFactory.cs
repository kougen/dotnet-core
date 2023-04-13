using Infrastructure.IO;
using Infrastructure.IO.Factories;
using Infrastructure.Logger;

namespace Implementation.IO.Factories
{
    public class IOFactory : IIOFactory
    {
        public IReader CreateReader(ILogger logger, IWriter writer)
        {
            return new Reader(logger, writer);
        }

        public IWriter CreateWriter(ILogger logger)
        {
            return new Writer(logger);
        }
    }
}