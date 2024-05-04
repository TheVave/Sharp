using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics._2d.Physics.CollisionManagement;
using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;

namespace SharpPhysics._2d.Physics
{
	/// <summary>
	/// The default 2d physics simulator. Based on the old unfinished 3d one.
	/// </summary>
	[Serializable]
	public class _2dPhysicsSimulator : IAny
	{

		/// <summary>
		/// The object to simulate
		/// </summary>
		public SimulatedObject2d ObjectToSimulate;

		/// <summary>
		/// Stops calculation
		/// </summary>
		public bool StopSignal = false;

		/// <summary>
		/// The signal to calculate physics if you are manually calling the tick function.
		/// </summary>
		public bool TickSignal = false;

		/// <summary>
		/// WARNING: this has a maximum of 1000
		/// </summary>
		public int TickSpeed = 200;

		/// <summary>
		/// Delay for checking ticks.
		/// Recommended to be high for background objects.
		/// If above DelayAmount then may yield strange results.
		/// </summary>
		public int WaitTime = 6;

		/// <summary>
		/// The most recent movement the object has taken.
		/// </summary>
		public _2dMovementRepresenter CurrentMovement { get; private set; } = new(new Position(0, 0));

		/// <summary>
		/// Momentum multiplier
		/// </summary>
		public int SpeedMultiplier = 1;

		/// <summary>
		/// What to execute at a collision
		/// </summary>
		public Action<SimulatedObject2d> ToExecuteAtCollision;

		/// <summary>
		/// The delay per physics engine tick
		/// </summary>
		public int DelayAmount;

		/// <summary>
		/// The amount of time that passes per simulation tick
		/// </summary>
		public double TimePerSimulationTick;

		/// <summary>
		/// If the program should do manual ticking pulses
		/// </summary>
		private readonly bool DoManualTicking = false;

		/// <summary>
		/// The latest collision info
		/// </summary>
		private CollisionData[]? resultFromCheckCollision;

		/// <summary>
		/// If the physics is beging run on the object
		/// </summary>
		public bool IsRunning;

		/// <summary>
		/// The object's rotation
		/// </summary>
		private double rotationalAmount = 0;

		public _2dPhysicsSimulator(SimulatedObject2d objectToSimulate) => ObjectToSimulate = objectToSimulate;

		public _2dPhysicsSimulator(SimulatedObject2d objectToSimulate, bool doManualTicking)
		{
			DoManualTicking = doManualTicking;
			ObjectToSimulate = objectToSimulate;
		}

		string posName;
		string rotName;
		string momName;
		string colName;
		string triName;


		internal void Tick()
		{
			// thread stuff
			// position calc thread
			new Thread(PosCalc)
			{
				Name = posName,
				IsBackground = true,
			}.Start();
			// rotation thread
			new Thread(RotCalc)
			{
				Name = rotName,
				IsBackground = true,
			}.Start();
			// momentum thread
			new Thread(MomCalc)
			{
				Name = momName,
				IsBackground = true,
			}.Start();
			// collision thread
			new Thread(ColCalc)
			{
				Name = colName,
				IsBackground = true,
			}.Start();
			// updating triangle position
			new Thread(TriCalc)
			{
				Name = triName,
				IsBackground = true,
			}.Start();
		}

		private void TriCalc()
		{
			Triangle actualTriangle;
			Triangle tri;
			for (int i = 0; i < ObjectToSimulate.ObjectMesh.ActualTriangles.Length; i++)
			{
				actualTriangle = ObjectToSimulate.ObjectMesh.ActualTriangles[i];
				//Triangle tri = ObjectToSimulate.ObjectMesh.ActualTriangles[i]/*.ShallowClone()*/;
				//ObjectToSimulate.ObjectMesh.MeshTriangles[i++] = tri.ScaleTriangle(ObjectToSimulate.Translation.ObjectScale.xSca, ObjectToSimulate.Translation.ObjectScale.ySca).
				//												 RotateByRadians(GenericMathUtils.DegreesToRadians(ObjectToSimulate.Translation.ObjectRotation.xRot)).
				//												 ShiftTriangle(new(ObjectToSimulate.Translation.ObjectPosition.X, ObjectToSimulate.Translation.ObjectPosition.Y));
				tri = new(actualTriangle.Vertex1, actualTriangle.Vertex2, actualTriangle.Vertex3, false);
				Triangle.ScaleTriangle(ObjectToSimulate.Translation.ObjectScale.XSca, ObjectToSimulate.Translation.ObjectScale.YSca, ref tri);
				Triangle.RotateByRadians(GenericMathUtils.DegreesToRadians(ObjectToSimulate.Translation.ObjectRotation.XRot), ref tri);
				Triangle.ShiftTriangle(ObjectToSimulate.Translation.ObjectPosition, tri);
				ObjectToSimulate.ObjectMesh.MeshTriangles[i] = tri;
			}
		}

		private void ColCalc()
		{
			resultFromCheckCollision = _2dCollisionManager.CheckIfCollidedWithObject(_2dWorld.SceneHierarchies[ObjectToSimulate.ObjectPhysicsParams.sceneID].Objects, ObjectToSimulate);
			if (resultFromCheckCollision.Length is not 0)
			{
				SimulatedObject2d collidedObject;
				for (int i = 0; i < resultFromCheckCollision.Length; i++)
				{
					collidedObject = resultFromCheckCollision[i].CollidedObject;
					ToExecuteAtCollision?.Invoke(collidedObject);
					_2dCollisionManager.SimulateCollision(ref ObjectToSimulate, resultFromCheckCollision[i].CollidedTriangle, resultFromCheckCollision[i].CollidedPoint, ref collidedObject);
				}

			}
		}

		private void MomCalc()
		{
			ObjectToSimulate.ObjectPhysicsParams.Velocity.X = GenericMathUtils.SubtractToZero(ObjectToSimulate.ObjectPhysicsParams.Velocity.X, ObjectToSimulate.ObjectPhysicsParams.SpeedResistance);

			ObjectToSimulate.ObjectPhysicsParams.Velocity.Y = GenericMathUtils.SubtractToZero(ObjectToSimulate.ObjectPhysicsParams.Velocity.Y, ObjectToSimulate.ObjectPhysicsParams.SpeedResistance);

			// set mesh points for collision
			for (int i = 0; i < ObjectToSimulate.ObjectMesh.MeshPointsX.Length; i++)
			{
				ObjectToSimulate.ObjectMesh.MeshPointsX[i] = ObjectToSimulate.ObjectMesh.MeshPointsActualX[i] + ObjectToSimulate.Translation.ObjectPosition.X;
				ObjectToSimulate.ObjectMesh.MeshPointsY[i] = ObjectToSimulate.ObjectMesh.MeshPointsActualY[i] + ObjectToSimulate.Translation.ObjectPosition.Y;
			}
		}

		private void RotCalc()
		{
			// update CurrentMovement value
			CurrentMovement.StartPosition = CurrentMovement.EndPosition;
			CurrentMovement.EndPosition = ObjectToSimulate.Translation.ObjectPosition;
			// new code for rotation similar to momentum and position.
			// may change. Ideas for rotational momentum impulse may be from https://phys.libretexts.org/Bookshelves/College_Physics/College_Physics_1e_(OpenStax)/10%3A_Rotational_Motion_and_Angular_Momentum/10.03%3A_Dynamics_of_Rotational_Motion_-_Rotational_Inertia
			// r in the upper link can be found by finding the area of the object, then working backward from the circle area equation, A = [pi]r^2, rearranged to r = [pi] / sqr(a).
			rotationalAmount = ObjectToSimulate.ObjectPhysicsParams.RotationalVelocity * TimePerSimulationTick;
			ObjectToSimulate.Translation.ObjectRotation.XRot += (float)(rotationalAmount);
			ObjectToSimulate.ObjectPhysicsParams.RotationalVelocity = GenericMathUtils.SubtractToZero(ObjectToSimulate.ObjectPhysicsParams.RotationalVelocity, ObjectToSimulate.ObjectPhysicsParams.RotResistance);
		}

		private void PosCalc()
		{
			// do standard calculations to find the displacement in a given direction
			ObjectToSimulate.Translation.ObjectPosition.X += ObjectToSimulate.ObjectPhysicsParams.Velocity.X;
			ObjectToSimulate.Translation.ObjectPosition.Y += ObjectToSimulate.ObjectPhysicsParams.Velocity.Y - ((9.8 * ObjectToSimulate.ObjectPhysicsParams.Mass) * (TimePerSimulationTick / 1000));

			// add velocity
			ObjectToSimulate.ObjectPhysicsParams.Velocity.X += (ObjectToSimulate.Translation.ObjectPosition - CurrentMovement.EndPosition).X;
			ObjectToSimulate.ObjectPhysicsParams.Velocity.Y += (ObjectToSimulate.Translation.ObjectPosition - CurrentMovement.EndPosition).Y - ((9.8 * ObjectToSimulate.ObjectPhysicsParams.Mass) * (TimePerSimulationTick / 1000));
		}

		internal void ThreadNameInit()
		{
			posName = $"Pos {ObjectToSimulate.Name}";
			rotName = $"Rot {ObjectToSimulate.Name}";
			momName = $"Vct {ObjectToSimulate.Name}";
			colName = $"Col {ObjectToSimulate.Name}";
			triName = $"Tri {ObjectToSimulate.Name}";
		}

		/// <summary>
		/// Things that need to happen before any 2dPhysicsSimulator.Tick calls
		/// </summary>
		internal void Prerequisites()
		{
			ThreadNameInit();
		}

		internal void StartPhysicsSimulator()
		{
			Prerequisites();
			Thread thread = new(() =>
			{
				DelayAmount = (int)Math.Ceiling(1000d / TickSpeed);
				TimePerSimulationTick = (DelayAmount / 1000);
				WaitTime = 2;
				while (true)
				{
					if (StopSignal) break;
					if (!DoManualTicking)
					{
						Tick();
					}
					else
					{
						if (TickSignal)
						{
							Tick();
							TickSignal = false;
						}
					}
					Task.Delay(DelayAmount).Wait();
				}
			})
			{
				Name = $"Physics thread",
				IsBackground = true
			};
			thread.Start();
		}
	}
}