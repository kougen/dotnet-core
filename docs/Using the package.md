There are two main ways to use the package. 
- Registering your classes in the same collection that you need for registering this module. (Recommended)
- Passing the `IServiceProvider` through the constructor.

## Registering your own classes

The advantage of using a Container is that let's say you have a class of which constructor takes 3 parameters:

```cs
internal class ExampleService
{
   public ExampleService(ServiceA servicA, SmallService smallService, BigService bigService)
   {
      // Do the assignments
   }
}
```

> **NOTE**: It is recommended to use [interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface) and then [register and resolve](https://github.com/joshika39/cs-tools/wiki/Register-and-resolve-with-inteface) them with the interface

Now if you register all of your services in the collection and register the `ExampleService` as well, then you can just call.

```cs
var collection = new ServiceCollection();  
// The registrations shoud go here 
var provider = collection.BuildServiceProvider();
var exService = provider.GetRequiredService<ExampleService>();
```

And everything will be injected for you. For example the [`Reader`](https://github.com/joshika39/cs-tools/blob/9a09d0442ba76e75eda543a26aab81d5ded3e7ec/_src/Implementation/IO/Reader.cs#L17-L22) from this package:

```cs
public Reader(ILogger logger, IWriter writer, IDataParser dataParser)
{
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _writer = writer ?? throw new ArgumentNullException(nameof(writer));
    _dataParser = dataParser ?? throw new ArgumentNullException(nameof(dataParser));
}
```
The there parameters are registered in the [module of the project](https://github.com/joshika39/cs-tools/blob/9a09d0442ba76e75eda543a26aab81d5ded3e7ec/_src/Implementation/Module/CoreModule.cs#L25-L31):

```cs
collection.AddScoped<ILogger, Logger.Logger>(_ => new Logger.Logger(Guid.NewGuid()));
collection.AddScoped<IWriter, Writer>();
collection.AddScoped<IReader, Reader>();
collection.AddScoped<IDataParser, DefaultDataParser>()
```

### Registering in a separate class
You can create a "Module" for your project by creating a new class and implementing the `IModule` from the `joshika39.Core`.

Let's say you have a `PdfService` and a `MyLogger` class that you want to register. First you need to create a *module* class: 

```cs
internal class MyModule : IModule 
{
    public void LoadModules(IServiceCollection collection)
    {
        // Do your class registrations
		
        // Read about lifecycles here: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines
        collection.AddSingleton<PdfService>();
        collection.AddScoped<MyLogger>();		
    }
}
```

Then you just have to call your module's `LoadModules` function:

```cs
var collection = new ServiceCollection();  
new CoreModule().LoadModules(collection, "reader-tests"); 
new MyModule().LoadModules(collection);
var provider = collection.BuildServiceProvider();
```
### Registering directly in the composition root
This is a the same if you take out the *core* of your `LoadModules` function from to [previous example](#Registering-in-a-separate-class)

```cs
var collection = new ServiceCollection();  
new CoreModule().LoadModules(collection, "reader-tests"); 

// Do your class registrations	
// Read about lifecycles here: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines
collection.AddSingleton<PdfService>();
collection.AddScoped<MyLogger>();	

var provider = collection.BuildServiceProvider();
```

## `IServiceProvider` in the constructor
Basically you only need to create you class and pass the `IServiceProvider` and then manually resolve the services

```cs
internal class ExampleService
{
   private readonly ServiceA _serviceA;
   private readonly SmallService _smallService;
   private readonly BigService _bigService;

   public ExampleService(IServiceProvider serviceProvider)
   {
      _serviceA = serviceProvider.GetRequiredService<ServiceA>();
      _smallService = serviceProvider.GetRequiredService<SmallService>();
      _bigService = serviceProvider.GetRequiredService<BigService>();
   }
}
```

And this is how to create the object:

```cs
var collection = new ServiceCollection();  
new CoreModule().LoadModules(collection, "reader-tests"); 
var provider = collection.BuildServiceProvider();

var exampleService = new ExampleService(provider);
```
## References
- [Dependency injection guidelines - .NET | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines)
- [Dependency injection in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-7.0)