# SharpPhysics #

![SharpPhysics logo](./logo.svg)

## SUMMARY ##

---------------------------------------------------------

SharpPhysics is a (somewhat small and 2d) physics/rendering library set up with Silk.net with OpenGL.

## BUILDING ##

---------------------------------------------------------

To build, do all the normal things to build, and make sure to copy over all .glsl files to [output directory]\Shaders,
and also copy over glfw.dll to the output directory.

## PLATFORM SUPPORT ##

---------------------------------------------------------

SharpPhysics only runs on Windows because the glfw.dll file isn't compatible with other Operating Systems.

### UPDATE ###

Soon it may be compatible with MacOS and Linux.

## RECENT UPDATES ##

---------------------------------------------------------

The camera is done.
I'm going to start work on collision. (soon)
I just finished a save game things, not yet loading saves though.
Soon I may make a small game with SP.

## PACKAGES ##

---------------------------------------------------------

This program makes use of four libaries, they are:
Newtonsoft.Json
Silk.Net
StbImageSharp
DeepCloner.

None of these were made by me.

### Newtonsoft.Json ###

---------------------------------------------------------
Newtonsoft.Json is a popular Json Serialization-Desirialization library for C#.
[Website](https://www.newtonsoft.com/json)
[Nuget](https://www.nuget.org/packages/Newtonsoft.Json)

### Silk.Net ###

---------------------------------------------------------
Silk.Net is a graphics bindings library.
[Website](https://dotnet.github.io/Silk.NET/)
[Github](https://github.com/dotnet/Silk.NET)
[Nuget](https://www.nuget.org/packages/Silk.NET)

### StbImageSharp ###

---------------------------------------------------------
StbImageSharp is a popular port of the stb_image.h libary.
[Github](https://github.com/StbSharp/StbImageSharp)
[Nuget](https://www.nuget.org/packages/StbImageSharp/)

### DeepCloner ###

---------------------------------------------------------
DeepCloner is a .net cloning libary for deep and shallow cloning.
[Github](https://github.com/force-net/DeepCloner)
[Nuget](https://www.nuget.org/packages/DeepCloner)