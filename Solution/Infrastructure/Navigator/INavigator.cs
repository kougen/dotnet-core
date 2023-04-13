using System;
using System.Collections.Generic;

namespace Infrastructure.Navigator
{
    public interface INavigator
    {
        int SelectedIndex { get; }
        int Show();

        void UpdateItems(IEnumerable<INavigatorElement> newList);
    }
}
