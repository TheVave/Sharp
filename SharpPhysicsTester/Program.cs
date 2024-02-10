using SharpPhysics;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Input;
double speed = .05;
MainRendererSGL.ObjectsToRender[1].objTextureLoc = "test.bmp";
MainRendererSGL.ObjectsToRender[0].objTextureLoc = "Enemy Thing.png";
MainRendererSGL.OnRender = MainRendererSGL.OnRender.Append((_2dSimulatedObject obj) =>
{

}).ToArray();
MainRendererSGL.OnUpdate = MainRendererSGL.OnUpdate.Append((_2dSimulatedObject obj) =>
{
	if (InputManager.IsKeyDown(VirtualKey.W))
	{
		obj.ObjectPhysicsParams.Momentum[1] = speed;
	}
	if (InputManager.IsKeyDown(VirtualKey.S)) 
	{
		obj.ObjectPhysicsParams.Momentum[1] = -speed;
	}
	if (InputManager.IsKeyDown(VirtualKey.A))
	{
		obj.ObjectPhysicsParams.Momentum[0] = -speed;
	}
	if (InputManager.IsKeyDown(VirtualKey.D))
	{
		obj.ObjectPhysicsParams.Momentum[0] = speed;
	}
}).ToArray();
MainRendererSGL.OnRender = MainRendererSGL.OnRender.Append((_2dSimulatedObject obj) =>
{

}).ToArray();
MainRendererSGL.OnUpdate = MainRendererSGL.OnUpdate.Append((_2dSimulatedObject obj) =>
{
	if (InputManager.IsKeyDown(VirtualKey.T))
	{
		obj.ObjectPhysicsParams.Momentum[1] = speed;
	}
	if (InputManager.IsKeyDown(VirtualKey.G))
	{
		obj.ObjectPhysicsParams.Momentum[1] = -speed;
	}
	if (InputManager.IsKeyDown(VirtualKey.F))
	{
		obj.ObjectPhysicsParams.Momentum[0] = -speed;
	}
	if (InputManager.IsKeyDown(VirtualKey.H))
	{
		obj.ObjectPhysicsParams.Momentum[0] = speed;
	}
}).ToArray();
MainRendererSGL.OnLoad += () =>
{
	MainRendererSGL.renderer.objectToRender[0].objToSim.StartPhysicsSimulation();
	MainRendererSGL.renderer.objectToRender[0].objToSim.ObjectPhysicsParams.GravityMultiplier = 0;
	MainRendererSGL.renderer.objectToRender[0].objToSim.ObjectPhysicsParams.SpeedResistance = 0.005;
	MainRendererSGL.renderer.objectToRender[1].objToSim.StartPhysicsSimulation();
	MainRendererSGL.renderer.objectToRender[1].objToSim.ObjectPhysicsParams.GravityMultiplier = 0;
	MainRendererSGL.renderer.objectToRender[1].objToSim.ObjectPhysicsParams.SpeedResistance = 0.005;
};
MainRendererSGL.InitRendering();