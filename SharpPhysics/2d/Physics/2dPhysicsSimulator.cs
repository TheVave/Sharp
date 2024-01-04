
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics._2d.Physics.CollisionManagement;
using SharpPhysics.Utilities.MathUtils;

namespace SharpPhysics._2d.Physics
{
	/// <summary>
	/// The default 2d physics simulator. Based on the old unfinished 3d one.
	/// </summary>
	public class _2dPhysicsSimulator
	{

		/// <summary>
		/// The object to simulate
		/// </summary>
		public _2dSimulatedObject ObjectToSimulate;

		/// <summary>
		/// bouncy multiplier
		/// </summary>
		public int ShocksMultiplier = 0;

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
		public int TickSpeed = 60;
		public _2dMovementRepresenter CurrentMovement { get; private set; } = new(new _2dPosition(0, 0));
		public int SpeedMultiplier = 1;
		public virtual void ExecuteAtCollision(_2dSimulatedObject hitObject, _2dSimulatedObject self) { }
		public int DelayAmount;
		public double TimePerSimulationTick = 0.001;

		/// <summary>
		/// The perceived radius of the circle from the object mesh
		/// see 108 - 110
		/// </summary>
		private double r;

		private readonly bool DoManualTicking = false;
		private CollisionData[]? resultFromCheckCollision;

		readonly SUVATEquations sUVATEquations = new SUVATEquations();

		double displacement;

		//calculating the position based on the moving position
		private double[] speedDirection = new double[2];

		private double rotationalAmount = 0;

		public _2dPhysicsSimulator(_2dSimulatedObject objectToSimulate) => ObjectToSimulate = objectToSimulate;


		public _2dPhysicsSimulator(_2dSimulatedObject objectToSimulate, int shocksMultiplier)
		{
			ShocksMultiplier = shocksMultiplier;
			ObjectToSimulate = objectToSimulate;
		}

		public _2dPhysicsSimulator(_2dSimulatedObject objectToSimulate, int shocksMultiplier, bool doManualTicking)
		{
			DoManualTicking = doManualTicking;
			ObjectToSimulate = objectToSimulate;
			ShocksMultiplier = shocksMultiplier;
		}

		internal void Tick()
		{
			resultFromCheckCollision = _2dCollisionManager.CheckIfCollidedWithObject(ObjectToSimulate.ObjectPhysicsParams.CollidableObjects, ObjectToSimulate);
			if (resultFromCheckCollision != null)
			{
				_2dSimulatedObject collidedObject;
				for (int i = 0; i < resultFromCheckCollision.Length; i++)
				{
					collidedObject = resultFromCheckCollision[i].CollidedObject;
					SimulateCollision(ref ObjectToSimulate, resultFromCheckCollision[i].ObjectToCheckIfCollidedMeshIndex, resultFromCheckCollision[i].objectToCheckMeshIndex, ref collidedObject);
				}

			}

			//Starting math for moving 1d

			sUVATEquations.T = TimePerSimulationTick;
			sUVATEquations.VS = ObjectToSimulate.ObjectPhysicsParams.Speed;
			sUVATEquations.A = ObjectToSimulate.ObjectPhysicsParams.SpeedAcceleration;

			displacement = sUVATEquations.NSWVSTA();

			//calculating the position based on the moving position
			speedDirection = ObjectToSimulate.ObjectPhysicsParams.Acceleration;

			// do standard calculations to find the displacement in a given direction
			ObjectToSimulate.Translation.ObjectPosition.xPos += ((speedDirection[0]) * displacement) + ObjectToSimulate.ObjectPhysicsParams.Momentum[0];
			ObjectToSimulate.Translation.ObjectPosition.yPos += (((speedDirection[1]) * displacement) + ObjectToSimulate.ObjectPhysicsParams.Momentum[1]) - ObjectToSimulate.ObjectPhysicsParams.GravityMultiplier * 9.8 * ObjectToSimulate.ObjectPhysicsParams.Mass;

			// add momentum
			ObjectToSimulate.ObjectPhysicsParams.Momentum[0] += (((speedDirection[0]) * displacement / sUVATEquations.T * ObjectToSimulate.ObjectPhysicsParams.Mass));
			ObjectToSimulate.ObjectPhysicsParams.Momentum[1] += ((speedDirection[1]) * displacement / sUVATEquations.T * ObjectToSimulate.ObjectPhysicsParams.Mass) - (ObjectToSimulate.ObjectPhysicsParams.GravityMultiplier * 9.8 * ObjectToSimulate.ObjectPhysicsParams.Mass);

			// update CurrentMovement value
			CurrentMovement.StartPosition = CurrentMovement.EndPosition;
			CurrentMovement.EndPosition = ObjectToSimulate.Translation.ObjectPosition;

			// new code for rotation similar to momentum and position.
			// may change. Ideas for rotation may be from https://phys.libretexts.org/Bookshelves/College_Physics/College_Physics_1e_(OpenStax)/10%3A_Rotational_Motion_and_Angular_Momentum/10.03%3A_Dynamics_of_Rotational_Motion_-_Rotational_Inertia
			// r in the upper link can be found by finding the area of the object, then working backward from the circle area equation, A = [pi]r^2, rearranged to r = r = [pi] / sqr(a).
			rotationalAmount = ((ObjectToSimulate.ObjectPhysicsParams.RotationalAcceleration + ObjectToSimulate.ObjectPhysicsParams.RotationalMomentum) * TimePerSimulationTick);
			ObjectToSimulate.Translation.ObjectRotation.xRot += (float)(rotationalAmount);
			ObjectToSimulate.ObjectPhysicsParams.RotationalMomentum = GenericMathUtils.SubtractToZero(ObjectToSimulate.ObjectPhysicsParams.RotationalAcceleration + ObjectToSimulate.ObjectPhysicsParams.RotationalMomentum, ObjectToSimulate.ObjectPhysicsParams.RotResistance);



			ObjectToSimulate.ObjectPhysicsParams.Momentum[0] = GenericMathUtils.SubtractToZero(ObjectToSimulate.ObjectPhysicsParams.Momentum[0], ObjectToSimulate.ObjectPhysicsParams.SpeedResistance);

			ObjectToSimulate.ObjectPhysicsParams.Momentum[1] = GenericMathUtils.SubtractToZero(ObjectToSimulate.ObjectPhysicsParams.Momentum[1], ObjectToSimulate.ObjectPhysicsParams.SpeedResistance);

			// set mesh points for collision
			for (int i = 0; i < ObjectToSimulate.ObjectMesh.MeshPointsX.Length; i++)
			{
				ObjectToSimulate.ObjectMesh.MeshPointsX[i] = ObjectToSimulate.ObjectMesh.MeshPointsActualX[i] + ObjectToSimulate.Translation.ObjectPosition.xPos;
				ObjectToSimulate.ObjectMesh.MeshPointsY[i] = ObjectToSimulate.ObjectMesh.MeshPointsActualY[i] + ObjectToSimulate.Translation.ObjectPosition.yPos;
			}



		}

		/// <summary>
		/// Applies a force on a specific point on a mesh line.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="meshLineIndex"></param>
		/// <param name="linePoint"></param>
		// Mostly (only) used for collisions
		internal void SimulateCollision(ref _2dSimulatedObject obj, int MeshLineIndexCollided, int MeshLineIndexCollider, ref _2dSimulatedObject collidedObject)
		{
			//_2dLine line =
			//	new(obj.ObjectMesh.MeshPoints[meshLineIndex], obj.ObjectMesh.MeshPoints[meshLineIndex + 1]);
			//obj.ObjectPhysicsParams.Momentum[0] -= 1;
			//obj.ApplyVectorMomentum(new _2dVector(new Angle(0), 1));
		}

		/// <summary>
		/// Things that need to happen before any 2dPhysicsSimulator.Tick calls
		/// </summary>
		internal void Prerequisites()
		{
			// solving for mesh stuff
			// finding a value for rotation.
			r = Math.PI / Math.Sqrt(MeshUtilities.PolygonArea(ObjectToSimulate.ObjectMesh.MeshPoints));
		}

		internal void StartPhysicsSimulator()
		{
			Prerequisites();
			Thread thread = new Thread(() =>
			{
				TimePerSimulationTick = ObjectToSimulate.ObjectPhysicsParams.TimeMultiplier / ObjectToSimulate.ObjectPhysicsParams.TicksPerSecond;
				DelayAmount = (int)Math.Ceiling(1000d / TickSpeed);
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
			});
			thread.IsBackground = true;
			thread.Start();
		}
	}
}