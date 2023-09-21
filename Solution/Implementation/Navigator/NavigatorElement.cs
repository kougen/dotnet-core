using System;
using Infrastructure.Navigator;

namespace Implementation.Navigator
{
    internal class NavigatorElement<T> : INavigatorElement<T>
    {
        public string DisplayValue { get; }
        public T Value { get; }
        public Action Callback { get; }
        
        public NavigatorElement(string displayValue, T value, Action callback)
        {
            DisplayValue = displayValue ?? throw new ArgumentNullException(nameof(displayValue));
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        public override string ToString()
        {
            return DisplayValue;
        }
    }
}
