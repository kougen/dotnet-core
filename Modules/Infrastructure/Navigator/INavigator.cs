using System.Collections.Generic;

namespace Infrastructure.Navigator
{
    public interface INavigator
    {
        int Show(List<INavigatorElement> items);
    }
}
