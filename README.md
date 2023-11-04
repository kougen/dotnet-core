# C# Tools Library

`joshika39.Core` meant to enhance the basic C# functionalities, like the `Console.Readline` or storing data in `json` format. The project is actively being updated. If you have any enchantment idea create an [Issue](https://github.com/joshika39/cs-tools/issues/new) w
## Developement
For the need of unit test, check the opened issue [Unit Tests](https://github.com/joshika39/cs-tools/issues/15)
- Deployement: [![CI-CD](https://github.com/joshika39/cs-tools/actions/workflows/modules-cicd.yml/badge.svg)](https://github.com/joshika39/cs-tools/actions/workflows/modules-cicd.yml)

# Setting up
## As a package
 - Download the `Core` package from [nuget.org](https://www.nuget.org/packages/joshika39-Core) via any Nuget package manager
 - Downlaod it from [github.com](https://github.com/joshika39/cs-tools/pkgs/nuget/joshika39-Core) via any Nuget package manager

## Manually
Downlad and unzip the [Core.zip](https://github.com/joshika39/cs-tools/releases/latest) file and [reference it](https://learn.microsoft.com/en-us/visualstudio/ide/how-to-add-or-remove-references-by-using-the-reference-manager?view=vs-2022) in the desired C# projects.

## How to start

First you have to create a new [`ServiceCollection`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollection?view=dotnet-plat-ext-7.0) and then create an instance from the `CoreModule` class.

Then call the `LoadModules` function of the `CoreModule` class with the desired ***namespace***. 

> **NOTE:** The namespace is the scope of your project which uses the `joshika39.Core` package

Finally you can use the implementations from the result of the `collection.BuildServiceProvider()` which has a type of [`IServiceProvider`](https://learn.microsoft.com/en-us/dotnet/api/system.iserviceprovider?view=net-7.0&viewFallbackFrom=dotnet-plat-ext-7.0)
### Example
```cs
var collection = new ServiceCollection();  
new CoreModule().LoadModules(collection, "reader-tests"); 

var provider = collection.BuildServiceProvider();
```

**Check the [Wiki](https://github.com/joshika39/cs-tools/wiki) for further details.**