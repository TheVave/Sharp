using Force.DeepCloner;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics._2d.Physics.CollisionManagement;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

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
		public int TickSpeed = 200;

		/// <summary>
		/// Delay for checking ticks.
		/// Recommended to be high for background objects.
		/// If above DelayAmount then may yeild strange results.
		/// </summary>
		public int WaitTime = 6;

		/// <summary>
		/// The most recent movement the object has taken.
		/// </summary>
		public _2dMovementRepresenter CurrentMovement { get; private set; } = new(new _2dPosition(0, 0));

		/// <summary>
		/// Momentum multiplier
		/// </summary>
		public int SpeedMultiplier = 1;

		/// <summary>
		/// What to execute at a collision
		/// </summary>
		public IExecuteAtCollision? ToExecuteAtCollision;

		/// <summary>
		/// The delay per physics engine tick
		/// </summary>
		public int DelayAmount;

		/// <summary>
		/// The amount of time that passes per simulation tick
		/// </summary>
		public double TimePerSimulationTick;

		/// <summary>
		/// The time the code takes to run per tick
		/// </summary>
		public static double TickActualTime;

		/// <summary>
		/// If the physics engine should see if a collision has happened.
		/// Used if a collision is detected to, when a collision happens to
		/// not accel the objects to the speed of light (very fast).
		/// </summary>
		public bool DetectCollision = true;

		/// <summary>
		/// The perceived radius of the circle from the object mesh
		/// </summary>
		private double r;

		/// <summary>
		/// If the program should do manual ticking pulses
		/// </summary>
		private readonly bool DoManualTicking = false;

		/// <summary>
		/// The latest collision info
		/// </summary>
		private CollisionData[]? resultFromCheckCollision;

		/// <summary>
		/// The suvatequasions instance
		/// </summary>
		readonly SUVATEquations sUVATEquations = new SUVATEquations();

		/// <summary>
		/// The latest displacement
		/// </summary>
		double displacement = 0;

		/// <summary>
		/// If the physics is beging run on the object
		/// </summary>
		public bool IsRunning;

		//calculating the position based on the moving position
		private double[] speedDirection = new double[2];

		/// <summary>
		/// The object's rotation
		/// </summary>
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

		string svtName;
		string posName;
		string rotName;
		string momName;
		string colName;
		string triName;

		internal void Tick()
		{
			// thread stuff
			// suvat thrd
			new Thread(SvtCalc)
			{
				Name = svtName,
				IsBackground = true,
			}.Start();
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
			for (int i = 0; i < ObjectToSimulate.ObjectMesh.ActualTriangles.Length; i++)
			{
				Triangle tri = ObjectToSimulate.ObjectMesh.ActualTriangles[i].DeepClone();
				ObjectToSimulate.ObjectMesh.MeshTriangles[i++] = tri.ScaleTriangle(ObjectToSimulate.Translation.ObjectScale.xSca, ObjectToSimulate.Translation.ObjectScale.ySca).
																 RotateByRadians(GenericMathUtils.DegreesToRadians(ObjectToSimulate.Translation.ObjectRotation.xRot)).
																 ShiftTriangle(new(ObjectToSimulate.Translation.ObjectPosition.X, ObjectToSimulate.Translation.ObjectPosition.Y));
			}
		}

		private void ColCalc()
		{
			if (DetectCollision)
				resultFromCheckCollision = _2dCollisionManager.CheckIfCollidedWithObject(ObjectToSimulate.ObjectPhysicsParams.CollidableObjects, ObjectToSimulate);
			if (resultFromCheckCollision.Length is not 0)
			{
				_2dSimulatedObject collidedObject;
				for (int i = 0; i < resultFromCheckCollision.Length; i++)
				{
					collidedObject = resultFromCheckCollision[i].CollidedObject;
					if (ToExecuteAtCollision is not null)
						ToExecuteAtCollision.Execute(collidedObject, ObjectToSimulate);
					_2dCollisionManager.SimulateCollision(ref ObjectToSimulate, resultFromCheckCollision[i].CollidedTriangle, resultFromCheckCollision[i].CollidedPoint, ref collidedObject);
				}

			}
		}

		private void MomCalc()
		{
			ObjectToSimulate.ObjectPhysicsParams.Momentum[0] = GenericMathUtils.SubtractToZero(ObjectToSimulate.ObjectPhysicsParams.Momentum[0], ObjectToSimulate.ObjectPhysicsParams.SpeedResistance);

			ObjectToSimulate.ObjectPhysicsParams.Momentum[1] = GenericMathUtils.SubtractToZero(ObjectToSimulate.ObjectPhysicsParams.Momentum[1], ObjectToSimulate.ObjectPhysicsParams.SpeedResistance);

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
			// r in the upper link can be found by finding the area of the object, then working backward from the circle area equation, A = [pi]r^2, rearranged to r = r = [pi] / sqr(a).
			rotationalAmount = ((ObjectToSimulate.ObjectPhysicsParams.RotationalAcceleration + ObjectToSimulate.ObjectPhysicsParams.RotationalMomentum) * TimePerSimulationTick);
			ObjectToSimulate.Translation.ObjectRotation.xRot += (float)(rotationalAmount);
			ObjectToSimulate.ObjectPhysicsParams.RotationalMomentum = GenericMathUtils.SubtractToZero(ObjectToSimulate.ObjectPhysicsParams.RotationalAcceleration + ObjectToSimulate.ObjectPhysicsParams.RotationalMomentum, ObjectToSimulate.ObjectPhysicsParams.RotResistance);
		}

		private void PosCalc()
		{
			speedDirection = ObjectToSimulate.ObjectPhysicsParams.Acceleration;

			// do standard calculations to find the displacement in a given direction
			ObjectToSimulate.Translation.ObjectPosition.X += ((speedDirection[0]) * displacement) + ObjectToSimulate.ObjectPhysicsParams.Momentum[0];
			ObjectToSimulate.Translation.ObjectPosition.Y += (((speedDirection[1]) * displacement) + ObjectToSimulate.ObjectPhysicsParams.Momentum[1]) - ((ObjectToSimulate.ObjectPhysicsParams.GravityMultiplier * 9.8 * ObjectToSimulate.ObjectPhysicsParams.Mass) * (TimePerSimulationTick / 10));

			// add momentum
			ObjectToSimulate.ObjectPhysicsParams.Momentum[0] += (((speedDirection[0]) * displacement / sUVATEquations.T * ObjectToSimulate.ObjectPhysicsParams.Mass));
			ObjectToSimulate.ObjectPhysicsParams.Momentum[1] += ((speedDirection[1]) * displacement / sUVATEquations.T * ObjectToSimulate.ObjectPhysicsParams.Mass) - ((ObjectToSimulate.ObjectPhysicsParams.GravityMultiplier * 9.8 * ObjectToSimulate.ObjectPhysicsParams.Mass) * (TimePerSimulationTick / 10));
		}

		private void SvtCalc()
		{
			sUVATEquations.T = TimePerSimulationTick;
			sUVATEquations.VS = ObjectToSimulate.ObjectPhysicsParams.Speed;
			sUVATEquations.A = ObjectToSimulate.ObjectPhysicsParams.SpeedAcceleration;

			displacement = sUVATEquations.NSWVSTA();
		}

		internal void ThreadNameInit()
		{
			svtName = $"Svt {ObjectToSimulate.Name}";
			posName = $"Pos {ObjectToSimulate.Name}";
			rotName = $"Rot {ObjectToSimulate.Name}";
			momName = $"Mom {ObjectToSimulate.Name}";
			colName = $"Col {ObjectToSimulate.Name}";
			triName = $"Tri {ObjectToSimulate.Name}";
		}

		/// <summary>
		/// Things that need to happen before any 2dPhysicsSimulator.Tick calls
		/// </summary>
		internal void Prerequisites()
		{
			// solving for mesh stuff
			// finding a value for rotation.
			r = Math.PI / Math.Sqrt(MeshUtilities.PolygonArea(ObjectToSimulate.ObjectMesh.MeshPoints));

			ThreadNameInit();
		}

		internal void StartPhysicsSimulator()
		{
			Prerequisites();
			Thread thread = new Thread(() =>
			{
				TimePerSimulationTick = (ObjectToSimulate.ObjectPhysicsParams.TicksPerSecond * SpeedMultiplier);
				DelayAmount = (int)Math.Ceiling(1000d / TickSpeed);
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
			});
			thread.Name = $"Physics thread";
			thread.IsBackground = true;
			thread.Start();
		}
	}
}