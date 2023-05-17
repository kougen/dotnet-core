using System.Collections.Generic;
using Infrastructure.IO;
using Infrastructure.Navigator;
using Infrastructure.Navigator.Factories;

namespace Implementation.Navigator.Factories
{
    public class NavigatorFactory : INavigatorFactory
    {
        public INavigator<T> CreateNavigator<T>(IWriter writer)
        {
            return new Navigator<T>(writer, new List<INavigatorElement<T>>());
        }

        public INavigator<T> CreateNavigator<T>(IWriter writer, IEnumerable<INavigatorElement<T>> navigatorElements)
        {
            return new Navigator<T>(writer, navigatorElements);
        }
    }
}