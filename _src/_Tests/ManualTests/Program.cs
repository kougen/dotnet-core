using Infrastructure.Application;
using Infrastructure.IO;
using Infrastructure.Navigator.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace ManualTests
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var services = new Core().LoadModules();

            using (var scope = services.CreateScope())
            {
                var writer = scope.ServiceProvider.GetService<IWriter>();
                var reader = scope.ServiceProvider.GetService<IReader>();
                var elemFact = scope.ServiceProvider.GetService<INavigatorElementFactory>();
                var navFact = scope.ServiceProvider.GetService<INavigatorFactory>();
            
                writer.PrintSystemDetails("joshika39", 
                    "Joshua Hegedus", 
                    "YQMHWO",
                    "Name of project",
                    "Some long description.");

                // var test = reader.ReadLine<int>(int.TryParse, "Kerek egy szamot");
                // var test2 = new List<INavigatorElement<int>>
                // {
                //     elemFact.CreateNavigatorElement("asd", 123),
                //     elemFact.CreateNavigatorElement("zxc", 2),
                //     elemFact.CreateNavigatorElement("qwe", 32),
                //     elemFact.CreateNavigatorElement("ret", 43)
                // };
                // var nav = navFact.CreateNavigator(writer, test2);
                // var res = nav.Show();
                // writer.WriteLine(MessageSeverity.Success, $"Res: {res}");
            }

            var repos = services.GetRequiredService<IApplicationSettings>().RepositoryPath;
            
        }
    }
}
