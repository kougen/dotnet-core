using System;
using Infrastructure.Navigator;

namespace Implementation.Navigator
{
    public class NavigatorElement : INavigatorElement
    {
        public string DisplayValue { get; }
        public Action Callback { get; }
        
        public NavigatorElement(string displayValue, Action callback)
        {
            DisplayValue = displayValue ?? throw new ArgumentNullException(nameof(displayValue));
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }
        
        public override string ToString()
        {
            return DisplayValue;
        }
    }
}
