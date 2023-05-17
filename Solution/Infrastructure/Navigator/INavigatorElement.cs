using System;

namespace Infrastructure.Navigator
{
    public interface INavigatorElement<T>
    {
        string DisplayValue { get; }
        T Value { get; }
        Action Callback { get; }
    }
}
