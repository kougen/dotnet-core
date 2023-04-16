# C# Tools Library

This project is revolves around the CS bachlers degree requrements which mainly includes:
- Object Oriented programming
- SOLID principles
- Class and Object diagrams (and other UML diagrams)
- Using sequential files

## Developement
For the need of unit test, check the opened issue [Unit Tests](https://github.com/joshika39/cs-tools/issues/15)
- Deployement: [![CI-CD](https://github.com/joshika39/cs-tools/actions/workflows/modules-cicd.yml/badge.svg)](https://github.com/joshika39/cs-tools/actions/workflows/modules-cicd.yml)


# Usage

You can easely use the reader or writer with the built-in Bootstrapper class like so:
```cs
    // IReader only
    Bootstrapper.Initialize(out var reader);

    // or with IWriter
    Bootstrapper.Initialize(out var reader, out var writer);
``` 

## IReader
`IReader` is the main part of the package, and its `ReadLine` function.

Check the [Wiki](https://github.com/joshika39/cs-tools/wiki/IReader) for further details.