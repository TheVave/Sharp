### SUMMARY ###
---------------------------------------------------------

SharpPhysics is a (somewhat small) (2d) physics library that is set up with Silk.net with OpenGL.
The renderer is not fully set up, but I'm working on it.

### BUILDING ###
---------------------------------------------------------

To build, do all the normal things to build, and make sure to copy over all .glsl files to [output directory]\Shaders, and make them with a .shdr file extention

### PLATFORM SUPPORT ###
---------------------------------------------------------

Currently SharpPhysics only runs on Windows, and I have not found the time to squash the dependancy for Windows in the Input folder.
If someone really wants to do it, you can open a PR for it.

### PERFORMANCE ###
---------------------------------------------------------

SharpPhysicsTester for example, is around 50 MB.
Memory, Barely any, unless you have a few thousand objects, where it's around 2 GB, but with one object rendering it's around 40 MB of mem.
CPU, Depends entirely on how many objects there are, Normal with one object is very low.