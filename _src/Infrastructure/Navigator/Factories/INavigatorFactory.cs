using System.Collections.Generic;
using Infrastructure.IO;

namespace Infrastructure.Navigator.Factories
{
    public interface INavigatorFactory
    {
        INavigator<T> CreateNavigator<T>(IWriter writer);
        INavigator<T> CreateNavigator<T>(IWriter writer, IEnumerable<INavigatorElement<T>> navigatorElements);
    }
}