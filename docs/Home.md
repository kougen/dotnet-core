**Welcome to the cs-tools wiki!**
# Overview


This toolkit contains several useful C# tools.
Here are some highlights:
- [`IDataParser`](https://github.com/joshika39/cs-tools/wiki/IDataParser)
- [`IReader`](https://github.com/joshika39/cs-tools/wiki/IReader)
- [`IConfigurationQuery`](https://github.com/joshika39/cs-tools/wiki/IConfigurationQuery)
- [`IRepository`](https://github.com/joshika39/cs-tools/wiki/IRepository)
## How to use

First you have to create a new [`ServiceCollection`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollection?view=dotnet-plat-ext-7.0) and then create an instance from the `CoreModule` class.

Then call the `LoadModules` function of the `CoreModule` class with the desired *namespace*. 

Finally you can use the implementations from the result of the `collection.BuildServiceProvider()` which has a type of [`IServiceProvider`](https://learn.microsoft.com/en-us/dotnet/api/system.iserviceprovider?view=net-7.0&viewFallbackFrom=dotnet-plat-ext-7.0)
### Example
```cs
var collection = new ServiceCollection();  
new CoreModule().LoadModules(collection, "reader-tests"); 

var provider = collection.BuildServiceProvider();
```

## References
- [Dependency injection guidelines - .NET | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines)
