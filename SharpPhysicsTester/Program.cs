using SharpPhysics;
using SharpPhysics.Input;
double speed = 5;
double camSpeed = 10;
MainRendererSGL.ObjectsToRender[1].objTextureLoc = "test.bmp";
MainRendererSGL.ObjectsToRender[0].objTextureLoc = "Enemy Thing.png";
MainRendererSGL.OnLoad += () =>
{
	MainRendererSGL.ObjectsToRender[0].objToSim.StartPhysicsSimulation();
	MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.GravityMultiplier = 0;
	MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.SpeedResistance = 0.5;
	//MainRendererSGL.ObjectsToRender[1].objToSim.StartPhysicsSimulation();
	MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.GravityMultiplier = 0;
	MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.SpeedResistance = 0.5;
	MainRendererSGL.ObjectsToRender[1].objToSim.RegisterToScene();
	MainRendererSGL.ObjectsToRender[0].objToSim.RegisterToScene();
	MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.CollidableObjects = MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.CollidableObjects.Append(MainRendererSGL.ObjectsToRender[1].objToSim).ToArray();
	MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.CollidableObjects = MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.CollidableObjects.Append(MainRendererSGL.ObjectsToRender[0].objToSim).ToArray();
	MainRendererSGL.Camera.obj = new();
	//MainRendererSGL.Camera.obj.StartPhysicsSimulation();
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