using SharpPhysics;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Input;
double speed = .05;
double camSpeed = 0.05;
MainRendererSGL.ObjectsToRender[1].objTextureLoc = "test.bmp";
MainRendererSGL.ObjectsToRender[0].objTextureLoc = "Enemy Thing.png";
MainRendererSGL.Use8BitStyleTextures = true;
MainRendererSGL.OnLoad += () =>
{
	MainRendererSGL.ObjectsToRender[0].objToSim.StartPhysicsSimulation();
	MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.GravityMultiplier = 0;
	MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.SpeedResistance = 0.005;
	MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.RotationalMomentum = 0.25;
	MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.RotResistance = 0;
	MainRendererSGL.ObjectsToRender[1].objToSim.StartPhysicsSimulation();
	MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.GravityMultiplier = 0;
	MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.SpeedResistance = 0.005;
	MainRendererSGL.Camera.obj = new();
	MainRendererSGL.Camera.obj.StartPhysicsSimulation();
	MainRendererSGL.Camera.obj.ObjectPhysicsParams.GravityMultiplier = 0;
	Thread thread = new Thread(() =>
	{
		while (true)
		{
			if (InputManager.IsKeyDown(VirtualKey.T))
			{
				MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.Momentum[1] = speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.G))
			{
				MainRendererSGL.renderer.ObjectsToRender[0].objToSim.ObjectPhysicsParams.Momentum[1] = -speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.F))
			{
				MainRendererSGL.renderer.ObjectsToRender[0].objToSim.ObjectPhysicsParams.Momentum[0] = -speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.H))
			{
				MainRendererSGL.renderer.ObjectsToRender[0].objToSim.ObjectPhysicsParams.Momentum[0] = speed;
			}

			if (InputManager.IsKeyDown(VirtualKey.W))
			{
				MainRendererSGL.renderer.ObjectsToRender[1].objToSim.ObjectPhysicsParams.Momentum[1] = speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.S))
			{
				MainRendererSGL.renderer.ObjectsToRender[1].objToSim.ObjectPhysicsParams.Momentum[1] = -speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.A))
			{
				MainRendererSGL.renderer.ObjectsToRender[1].objToSim.ObjectPhysicsParams.Momentum[0] = -speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.D))
			{
				MainRendererSGL.renderer.ObjectsToRender[1].objToSim.ObjectPhysicsParams.Momentum[0] = speed;
			}

			if (InputManager.IsKeyDown(VirtualKey.UP_ARROW))
			{
				MainRendererSGL.Camera.obj.ObjectPhysicsParams.Momentum[1] = camSpeed;
			}
			if (InputManager.IsKeyDown(VirtualKey.DOWN_ARROW))
			{
				MainRendererSGL.Camera.obj.ObjectPhysicsParams.Momentum[1] = -camSpeed;
			}
			if (InputManager.IsKeyDown(VirtualKey.LEFT_ARROW))
			{
				MainRendererSGL.Camera.obj.ObjectPhysicsParams.Momentum[0] = -camSpeed;
			}
			if (InputManager.IsKeyDown(VirtualKey.RIGHT_ARROW))
			{
				MainRendererSGL.Camera.obj.ObjectPhysicsParams.Momentum[0] = camSpeed;
			}
		}
	});
	thread.Start();
};
MainRendererSGL.InitRendering();