# SharpPhysics #

![SharpPhysics logo](./logo.svg)

## SUMMARY ##

---------------------------------------------------------

SharpPhysics is a (somewhat small and 2d) physics/rendering library set up with Silk.net with OpenGL.

## NOTICE ##

---------------------------------------------------------
SharpPhysics is under heavy development!!!
It should not be used in any production projects!

## BUILDING ##

---------------------------------------------------------

To build, do all the normal things to build, and make sure to copy over all .glsl files to [output directory]\Shaders,
and also copy over glfw.dll to the output directory.

## PLATFORM SUPPORT ##

---------------------------------------------------------

SharpPhysics runs on Windows, Mac, and Linux.

## RECENT UPDATES ##

---------------------------------------------------------

I just finished most of the UI stuff, I'll come back to it though.
What I'm doing next: Making test game

## PACKAGES ##

---------------------------------------------------------

This program makes use of four libaries, they are:
Newtonsoft.Json
Silk.Net
StbImageSharp
DeepCloner
GLFW (Written in C).

None of these were made by me.

### Newtonsoft.Json ###

---------------------------------------------------------
Newtonsoft.Json is a popular Json Serialization-Desirialization library for C#. <br>
[Website](https://www.newtonsoft.com/json) <brs>
[Nuget](https://www.nuget.org/packages/Newtonsoft.Json) <br> 

### Silk.Net ###

---------------------------------------------------------
Silk.Net is a graphics bindings library. <br>
[Website](https://dotnet.github.io/Silk.NET/) <br>
[Github](https://github.com/dotnet/Silk.NET) <br>
[Nuget](https://www.nuget.org/packages/Silk.NET) <br>

### StbImageSharp ###

---------------------------------------------------------
StbImageSharp is a popular port of the stb_image.h libary. <br>
[Github](https://github.com/StbSharp/StbImageSharp) <br>
[Nuget](https://www.nuget.org/packages/StbImageSharp/) <br>

### DeepCloner ###

---------------------------------------------------------
DeepCloner is a .net cloning libary for deep and shallow cloning. <br>
[Github](https://github.com/force-net/DeepCloner) <br>
[Nuget](https://www.nuget.org/packages/DeepCloner) <br>

### GLFW ###

---------------------------------------------------------
GLFW is a popular cross-platform OpenGL library written in C, and I'm <br>
communicating with it through Silk.NET. (NOTE: This was not originally made by me!) <br>
[Website](https://GLFW.org)
[Github](https://github.com/glfw/glfw)
