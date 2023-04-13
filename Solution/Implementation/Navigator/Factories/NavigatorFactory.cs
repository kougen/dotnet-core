using System.Collections.Generic;
using Infrastructure.IO;
using Infrastructure.Navigator;
using Infrastructure.Navigator.Factories;

namespace Implementation.Navigator.Factories
{
    public class NavigatorFactory : INavigatorFactory
    {
        public INavigator CreateNavigator(IWriter writer)
        {
            return new Navigator(writer, new List<INavigatorElement>());
        }

        public INavigator CreateNavigator(IWriter writer, IEnumerable<INavigatorElement> navigatorElements)
        {
            return new Navigator(writer, new List<INavigatorElement>());
        }
    }
}