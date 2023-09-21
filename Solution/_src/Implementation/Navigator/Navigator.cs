using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Infrastructure.IO;
using Infrastructure.Navigator;

namespace Implementation.Navigator
{
    internal class Navigator<T> : INavigator<T>
    {
        private readonly IWriter _writer;
        private IEnumerable<INavigatorElement<T>> _items;

        public Navigator(IWriter writer, IEnumerable<INavigatorElement<T>> items)
        {
            SelectedIndex = 0;
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public int SelectedIndex { get; private set; }

        public T Show()
        {
            if (_items == null || !_items.Any())
            {
                _writer.WriteLine(MessageSeverity.Error, "Navigator list is empty!");
                return default;
            }
            var selectedIndex = 0;
            
            ConsoleKeyInfo keyInfo;
            var itemList = _items.ToList();
            do
            {
                Console.Clear();
                Console.WriteLine("Select an option:");
                for (var i = 0; i < itemList.Count; i++)
                {
                    Console.Write(i == selectedIndex ? ">> " : "   ");
                    Console.WriteLine(itemList[i]);
                }
                keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow when selectedIndex > 0:
                        selectedIndex--;
                        break;
                    case ConsoleKey.DownArrow when selectedIndex < itemList.Count - 1:
                        selectedIndex++;
                        break;
                }

            } while (keyInfo.Key != ConsoleKey.Enter);
            
            Console.Clear();
            _writer.RestoreTerminalState();
            itemList[selectedIndex].Callback();
            SelectedIndex = selectedIndex;
            
            return itemList[SelectedIndex].Value;
        }

        public void UpdateItems(IEnumerable<INavigatorElement<T>> newList)
        {
            SelectedIndex = 0;
            _items = newList;
        }
    }
}
