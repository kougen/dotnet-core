using System;
using System.Collections.Generic;
using Implementation.Navigator;
using Implementation.Navigator.Factories;
using Infrastructure.Navigator;
using Infrastructure.Navigator.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace ManualTests
{
    internal class Dump
    {
        private void Dumper()
        {
            Console.ReadLine();
            var provider = new Core().LoadModules();
            var elemFact = provider.GetService<INavigatorElementFactory>();
            // var test = _reader.ReadAllLines("Adjon meg hosszu szoveget");
            var elements = new List<INavigatorElement<string>>()
            {
                elemFact.CreateNavigatorElement("asd", "asdValue"),
                elemFact.CreateNavigatorElement("dsa", "dsaValue"),
                elemFact.CreateNavigatorElement("qwe", "qweValue"),
                elemFact.CreateNavigatorElement("ewq", "ewqValue")
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