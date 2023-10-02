using System;

namespace Infrastructure.Navigator.Factories
{
    public interface INavigatorElementFactory
    {
        INavigatorElement<T> CreateNavigatorElement<T>(string displayText, T value);
        INavigatorElement<T> CreateNavigatorElement<T>(string displayText, T value, Action callback);
    }
}
