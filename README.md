# SharpPhysics #

![SharpPhysics logo](./logo.svg)

## SUMMARY ##

---------------------------------------------------------

SharpPhysics is a (somewhat small) (2d) physics library set up with Silk.net with OpenGL.

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
I'm going to start work on collision.
See you around.

## PACKAGES ##

--------------------------------------------------------;

This program makes use of [Silk.NET](https://github.com/dotnet/Silk.NET).
This program also makes use of [StbImageSharp](https://github.com/StbSharp/StbImageSharp), for texture info.
Finally, this program uses [DeepCloner](https://github.com/force-net/DeepCloner), for deep copies in the networking library.

## THINGS YOU CAN DO TO HELP ##

--------------------------------------------------------;

The main thing I haven't gotten around to yet is Platform Support stuff.
If someone builds glfw for Mac and Linux, then I'll put it into SP.
Also, If someone could make a PR for an Input folder that uses Silk input,
that would be useful too!
