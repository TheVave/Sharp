
using SharpPhysics;
using SharpPhysics._2d;
using SharpPhysics._2d.ObjectRepresentation.Hierarchies;
using SharpPhysics.Input;
using SharpPhysics.UI.UIElements;
using SharpPhysics.UI.UIHierarchy;
double speed = 5;
double camSpeed = 0.05;

_2dWorld.SceneHierarchies[0].Objects = [new(), new()];

_2dWorld.SceneHierarchies[0].Objects[1].objTextureLoc = "test.bmp";
_2dWorld.SceneHierarchies[0].Objects[0].objTextureLoc = "Enemy Thing.png";
UIRoot.Windows = [new("Debug", [new Label("Example")])];
UIRoot.NonWindowedObjects = [
	new Button()
	{
		Label = "Example close button",
		OnClick = () =>
		{
			Environment.Exit(4);
		}

	}
];
MainRendererSGL.OnLoad += () =>
{
	MainRendererSGL.ObjectsToRender[1].objToSim.RegisterToScene();
	MainRendererSGL.ObjectsToRender[0].objToSim.RegisterToScene();
	MainRendererSGL.Camera.obj = new();
	MainRendererSGL.Camera.obj.RegisterToScene();
	MainRendererSGL.ObjectsToRender[0].objToSim.StartPhysicsSimulation();
	MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.SpeedResistance = 0.5;
	//MainRendererSGL.ObjectsToRender[1].objToSim.StartPhysicsSimulation();
	MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.SpeedResistance = 0.5;
	MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.sceneID = 0;
	MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.sceneID = 0;
	SaveHelper.SaveObjects("MainSave");
	//MainRendererSGL.Camera.obj.StartPhysicsSimulation();
	Thread thread = new Thread(() =>
	{
		while (true)
		{
			if (InputManager.IsKeyDown(VirtualKey.T))
			{
				MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.Velocity.VelocityY = speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.G))
			{
				MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.Velocity.VelocityY = -speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.F))
			{
				MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.Velocity.VelocityX = -speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.H))
			{
				MainRendererSGL.ObjectsToRender[0].objToSim.ObjectPhysicsParams.Velocity.VelocityX = speed;
			}

			if (InputManager.IsKeyDown(VirtualKey.W))
			{
				MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.Velocity.VelocityY = speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.S))
			{
				MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.Velocity.VelocityY = -speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.A))
			{
				MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.Velocity.VelocityX = -speed;
			}
			if (InputManager.IsKeyDown(VirtualKey.D))
			{
				MainRendererSGL.ObjectsToRender[1].objToSim.ObjectPhysicsParams.Velocity.VelocityX = speed;
			}

			if (InputManager.IsKeyDown(VirtualKey.UP_ARROW))
			{
				MainRendererSGL.Camera.obj.ObjectPhysicsParams.Velocity.VelocityY = camSpeed;
			}
			if (InputManager.IsKeyDown(VirtualKey.DOWN_ARROW))
			{
				MainRendererSGL.Camera.obj.ObjectPhysicsParams.Velocity.VelocityY = -camSpeed;
			}
			if (InputManager.IsKeyDown(VirtualKey.LEFT_ARROW))
			{
				MainRendererSGL.Camera.obj.ObjectPhysicsParams.Velocity.VelocityX = -camSpeed;
			}
			if (InputManager.IsKeyDown(VirtualKey.RIGHT_ARROW))
			{
				MainRendererSGL.Camera.obj.ObjectPhysicsParams.Velocity.VelocityX = camSpeed;
			}
		}
	});
	thread.Start();
};
MainRendererSGL.InitRendering();