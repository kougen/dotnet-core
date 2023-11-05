The recommended Microsoft approach is to use interfaces. You can register something to the collection with the following syntax:

`collection.AddSingleton<TInterface, TImplementation>();`

The advantage of this is that you can create two implementation for the same interface. We can see an example in a game where there needed to be a [WinForms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/overview/?view=netdesktop-7.0) and a [WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/?view=netdesktop-7.0) project. These frameworks are not quite compatible with each other.

There is a [ITileFactory](https://github.com/joshika39/dotnet-bomber-game/blob/main/Bomber.BL/Tiles/Factories/ITileFactory.cs) which has two different implementation one for each framework:
- [FormsTileFactory](https://github.com/joshika39/dotnet-bomber-game/blob/main/Bomber.UI.Forms/Tiles/Factories/FormsTileFactory.cs)
- [WpfTileFactory](https://github.com/joshika39/dotnet-bomber-game/blob/main/Bomber.UI.WPF/Tiles/Factories/WpfTileFactory.cs)

These are separately registered for their specific project, and this way, the core backend logic can reference the interface not caring about which is the implementation behind it.

### Example

```cs
public interface IExampleSerice

internal class ExampleService : IExampleSerice
{
   public ExampleService(IServiceA servicA, ISmallService smallService, IBigService bigService)
   {
      // Do the assignments
   }
}
```

Registering and using it:

```cs
var collection = new ServiceCollection();  
collection.AddSingleton<IExampleService, ExampleService>()
var provider = collection.BuildServiceProvider();
var exService = provider.GetRequiredService<IExampleService>();
```

### Real life example

We also follow this style in the [module of the project](https://github.com/joshika39/cs-tools/blob/9a09d0442ba76e75eda543a26aab81d5ded3e7ec/_src/Implementation/Module/CoreModule.cs#L25-L31):

```cs
collection.AddScoped<ILogger, Logger.Logger>(_ => new Logger.Logger(Guid.NewGuid()));
collection.AddScoped<IWriter, Writer>();
collection.AddScoped<IReader, Reader>();
collection.AddScoped<IDataParser, DefaultDataParser>()
```

## References
- [Dependency injection guidelines - .NET | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines)
- [Dependency injection in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-7.0)