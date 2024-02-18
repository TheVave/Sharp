
using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics._2d.Objects;
using SharpPhysics._2d.Physics;
using SharpPhysics._2d.Raycasting;
using SharpPhysics.Simulation.ObjectHierarchy;
using SharpPhysics.Utilities.MISC.Errors;
using System.Text.Json.Serialization;

namespace SharpPhysics._2d.ObjectRepresentation
{
	[Serializable]
	public class SimulatedObject2d
	{
		/// <summary>
		/// The object name
		/// </summary>
		public string Name = "Object";

		/// <summary>
		/// The object's mesh
		/// </summary>
		public Mesh ObjectMesh { get; set; }

		/// <summary>
		/// Simulate physics, you can still include this object in collidable arrays
		/// </summary>
		public bool SimulatePhysics = true;

		/// <summary>
		/// The parameters that tell the physics engine how to behave.
		/// </summary>
		public PhysicsParams2d ObjectPhysicsParams;

		/// <summary>
		/// The translation of the object, with the position, rotation, and scale of the object.
		/// </summary>
		public Translation2d Translation;

		/// <summary>
		/// Creates a new _2dSimulatedObject and registers it to the simulation hierarchy in scene 1.
		/// </summary>
		/// <param name="objectMesh"></param>
		/// <param name="objectPhysicsParams"></param>
		/// <param name="translation"></param>
		public SimulatedObject2d(Mesh objectMesh, PhysicsParams2d objectPhysicsParams, Translation2d translation)
		{
			ObjectMesh = objectMesh;
			ObjectPhysicsParams = objectPhysicsParams;
			Translation = translation;
			try
			{
				SimulationHierarchy.Hierarchies[0].Objects = [.. SimulationHierarchy.Hierarchies[0].Objects, this];
			}
			catch (Exception e)
			{
				ErrorHandler.ThrowError("Error, Unknown error, _2dSimulatedObject.cs _2dSimulatedObject(Mesh,_2dPhysicsParams,_2dTranslation), exact error: " + e, true);
			}
		}

		/// <summary>
		/// Creates a new _2dSimulatedObject and registers it to the simulation hierarchy in scene 1.
		/// </summary>
		/// <param name="objectMesh"></param>
		/// <param name="objectPhysicsParams"></param>
		/// <param name="translation"></param>
		public SimulatedObject2d()
		{
			ObjectMesh = _2dBaseObjects.LoadSquareMesh();
			ObjectPhysicsParams = new PhysicsParams2d();
			Translation = new Translation2d();
			//SimulationHierarchy.Hierarchies[0].Objects = SimulationHierarchy.Hierarchies[0].Objects.Append(this).ToArray();
		}

		/// <summary>
		/// Registers the object to a scene.
		/// </summary>
		/// <param name="scene"></param>
		public void RegisterToScene(int scene)
		{
			SimulationHierarchy.Hierarchies[scene].Objects = [.. SimulationHierarchy.Hierarchies[0].Objects, this];
		}

		/// <summary>
		/// Registers the object to scene 1.
		/// </summary>
		public void RegisterToScene()
		{
			SimulationHierarchy.Hierarchies[0].Objects = [.. SimulationHierarchy.Hierarchies[0].Objects, this];
		}

		public void ApplyVectorVelocity(_2dVector force)
		{
			_2dLine forceLine = force.ToLine();
			if (ObjectPhysicsParams.Velocity.VelocityX < forceLine.XEnd) ObjectPhysicsParams.Velocity.VelocityX = 0;
			else ObjectPhysicsParams.Velocity.VelocityX = forceLine.XEnd;
			if (ObjectPhysicsParams.Velocity.VelocityY < forceLine.YEnd) ObjectPhysicsParams.Velocity.VelocityY = 0;
			else ObjectPhysicsParams.Velocity.VelocityY = forceLine.YEnd;
		}

		/// <summary>
		/// Start the physics simulation for the object based on ObjectPhysicsParams
		/// </summary>
		/// <returns></returns>
		public _2dPhysicsSimulator StartPhysicsSimulation()
		{
			_2dPhysicsSimulator physicsSimulator = new(this);
			try
			{
				physicsSimulator.StartPhysicsSimulator();
			}
			catch (Exception e) 
			{
				ErrorHandler.ThrowError("Error, Unknown error, _2dSimulatedObject.cs, StartPhysicsSimulation() exact error: " + e, true);
			}
			return physicsSimulator;
		}
	}
}
