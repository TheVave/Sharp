using SharpPhysics;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Input;
double speed = .1;
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
MainRendererSGL.OnLoad += () =>
{
	MainRendererSGL.renderer.objectToRender[0].objToSim.StartPhysicsSimulation();
	MainRendererSGL.renderer.objectToRender[0].objToSim.ObjectPhysicsParams.GravityMultiplier = 0;
};
MainRendererSGL.InitRendering();