using System;
using System.Collections.Generic;

namespace Infrastructure.Navigator
{
    public interface INavigator<T>
    {
        int SelectedIndex { get; }
        T Show();
        void UpdateItems(IEnumerable<INavigatorElement<T>> newList);
    }
}
