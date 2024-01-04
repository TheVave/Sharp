using SharpPhysics.Renderer;

new Thread(() =>
{
	while (true)
	{
		Task.Delay(16).Wait();
		if (SharpPhysics.Input.InputManager.IsKeyDown(SharpPhysics.Input.VirtualKey.ESCAPE))
		{
			Environment.Exit(0);
		}
	}
}).Start();
MainRenderer.InitRendering();