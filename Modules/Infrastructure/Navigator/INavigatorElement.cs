using System;

namespace Infrastructure.Navigator
{
    public interface INavigatorElement
    {
        string DisplayValue { get; }
        
        Action Callback { get; }
    }
}
