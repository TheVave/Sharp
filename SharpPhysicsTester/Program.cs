using SharpPhysics;
using SharpPhysics.Input;
using SharpPhysics.Input.Keyboard;
using SharpPhysics.Renderer;

new Thread(() =>
{

	while (true)
	{
		if (InputManager.IsKeyDown(VirtualKey.ESCAPE)) Environment.Exit(0);
		else Task.Delay(60);
	}
}).Start();

MainRenderer.InitRendering();
