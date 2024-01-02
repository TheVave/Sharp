using SharpPhysics;
using SharpPhysics.Renderer;

new Thread(() =>
{
	if (SharpPhysics.Input.InputManager.IsKeyDown(SharpPhysics.Input.VirtualKey.ESCAPE))
	{
		Environment.Exit(0);
	}
}).Start();
MainRenderer.InitRendering();