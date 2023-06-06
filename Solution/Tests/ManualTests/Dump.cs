using System;
using System.Collections.Generic;
using Implementation.Navigator;
using Implementation.Navigator.Factories;
using Infrastructure.Navigator;

namespace ManualTests
{
    public class Dump
    {
        private void Dumper()
        {
            Console.ReadLine();

            // var test = _reader.ReadAllLines("Adjon meg hosszu szoveget");
            var elements = new List<INavigatorElement<string>>()
            {
                new NavigatorElement<string>("asd", "asdValue"),
                new NavigatorElement<string>("dsa", "dsaValue"),
                new NavigatorElement<string>("qwe", "qweValue"),
                new NavigatorElement<string>("ewq", "ewqValue")
            };
            // var nav = new NavigatorFactory().CreateNavigator(_writer, elements);
            
            // var res = nav.Show();
            // _writer.WriteLine(res);
            // var people = _reader.ReadLine<TestPersonClass>(
            //     TestPersonClass.TryParse, 
            //     "Kerem az emberek adatait tabulatorral elvalasztva\nNev\tKor\tMagassag\t\n");
            //
            // foreach (var person in people)
            // {
            //     Console.WriteLine($"{person.Name}: {person.Age} eves es {person.Height}cm magas");
            // }
        }
    }
}