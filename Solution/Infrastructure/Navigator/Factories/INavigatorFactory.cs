using System.Collections.Generic;
using Infrastructure.IO;

namespace Infrastructure.Navigator.Factories
{
    public interface INavigatorFactory
    {
        INavigator CreateNavigator(IWriter writer);
        INavigator CreateNavigator(IWriter writer, IEnumerable<INavigatorElement> navigatorElements);
    }
}