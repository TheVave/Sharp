### SUMMARY ###
---------------------------------------------------------

SharpPhysics is a (somewhat small) (2d) physics library that is set up with Silk.net with OpenGL.
The renderer is not fully set up, but I'm working on it.

### BUILDING ###
---------------------------------------------------------

To build, do all the normal things to build, and make sure to copy over all .glsl files to [output directory]\Shaders, and make them with a .shdr file extention,
and also copy over glfw.dll to the output directory.

### PLATFORM SUPPORT ###
---------------------------------------------------------

Currently SharpPhysics only runs on Windows, because the glfw.dll file isn't compatible with other Operating Systems.

### PERFORMANCE ###
---------------------------------------------------------

SharpPhysicsTester for example, is around 50 MB.
Memory, Barely any, unless you have a few thousand objects, where it's around 2 GB, but with one object rendering it's around 40 MB of mem.
CPU, Depends entirely on how many objects there are, Normal with one object is very low.

### RECENT UPDATES ###
---------------------------------------------------------

I've recently made the(singular) square move, and made the app no longer crash running in a non-windows OS,
it just... Still needs the glfw.=>DLL<=, so I can't make that run correctly on Android, which is the OS I was going for right now.

### DOCS ###
---------------------------------------------------------

sry. nrn.