using System;
using Infrastructure.Navigator;
using Infrastructure.Navigator.Factories;

namespace Implementation.Navigator.Factories
{
    internal class NavigatorElementFactory : INavigatorElementFactory
    {

        public INavigatorElement<T> CreateNavigatorElement<T>(string displayText, T value)
        {
            return new NavigatorElement<T>(displayText, value, () => { });
        }
        public INavigatorElement<T> CreateNavigatorElement<T>(string displayText, T value, Action callback)
        {
            return new NavigatorElement<T>(displayText, value, callback);
        }
    }
}
