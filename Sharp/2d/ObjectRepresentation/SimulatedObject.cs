using Sharp._2d.ObjectRepresentation.Translation;
using Sharp._2d.Objects;
using Sharp._2d.Physics;
using Sharp._2d.Raycasting;
using Sharp.StrangeDataTypes;
using Sharp.Utilities.MISC;
using Sharp.Utilities.MISC.Unsafe;
using System.Runtime.InteropServices;
using System.Text;

namespace Sharp._2d.ObjectRepresentation
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SimulatedObject2d : ISizeGettable, IAny
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
		/// Simulate physics. you can still include this object in collidable arrays.
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
				_2dWorld.SceneHierarchies[0].Objects = [.. _2dWorld.SceneHierarchies[0].Objects, this];
			}
			catch (Exception e)
			{
				//5
				ErrorHandler.ThrowError(5, true);
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
			_2dWorld.SceneHierarchies[scene].Objects = [.. _2dWorld.SceneHierarchies[scene].Objects, this];
		}

		/// <summary>
		/// Registers the object to scene 1.
		/// </summary>
		public void RegisterToScene()
		{
			_2dWorld.SceneHierarchies[0].Objects = [.. _2dWorld.SceneHierarchies[0].Objects, this];
		}

		public void ApplyVectorVelocity(_2dVector force)
		{
			_2dLine forceLine = force.ToLine();
			if (ObjectPhysicsParams.Velocity.X < forceLine.XEnd) ObjectPhysicsParams.Velocity.X = 0;
			else ObjectPhysicsParams.Velocity.X = forceLine.XEnd;
			if (ObjectPhysicsParams.Velocity.Y < forceLine.YEnd) ObjectPhysicsParams.Velocity.Y = 0;
			else ObjectPhysicsParams.Velocity.Y = forceLine.YEnd;
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
				// 5
				ErrorHandler.ThrowError(5, true);
			}
			return physicsSimulator;
		}

		public int GetSize() =>
			// Name param size
			Encoding.Unicode.GetByteCount(Name) +
			// SimulatePhysics param
			sizeof(bool) +
			// complex: ObjectMesh
			ObjectMesh.GetSize() +
			// Complex: ObjectPhysicsParams
			ObjectPhysicsParams.GetSize() +
			// Complex: Translation
			Translation.GetSize();
	}
}
