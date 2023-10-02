using System;

namespace Infrastructure.Navigator
{
    public interface INavigatorElement<out T>
    {
        string DisplayValue { get; }
        T Value { get; }
        Action Callback { get; }
    }
}
