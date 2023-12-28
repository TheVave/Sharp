using SharpPhysics;

namespace SharpPhysics
{
	public struct _2dPhysicsParams
	{
		/// <summary>
		/// Simulate physics, you can still include this object in collidable arrays
		/// </summary>
		public bool SimulatePhysics = true;

		/// <summary>
		/// the gravity multiplier, 1 to do normal and 0 for none, 9.8 dist/s
		/// </summary>
		public int GravityMultiplier = 1;

		/// <summary>
		/// Increases the accuracy of the physics simulation but decreases the porformance
		/// </summary>
		public int TicksPerSecond = 20;

		/// <summary>
		/// The objects that the object to simulate can collide with
		/// </summary>
		public SimulatedObject[] CollidableObjects = new SimulatedObject[0];

		/// <summary>
		/// the time multiplier
		/// </summary>
		public double TimeMultiplier = 1;

		/// <summary>
		/// Mass of the object
		/// </summary>
		public double Mass = 1f;

		/// <summary>
		/// WARNING: unused
		/// </summary>
		public double SpeedResistance = 0;

		public double SpeedAcceleration = 0;

		/// <summary>
		/// the rotation resistance of the object
		/// </summary>
		public double RotResistance = 0;

		/// <summary>
		/// Speed in SpeedDirection
		/// </summary>
		public double Speed = 1;

		/// <summary>
		/// 3d direction using double[2]
		/// eg. SpeedDirection = new double { 10,0 }
		/// the object would go ten units in the x direction and 10 in the z direction
		/// </summary>
		public double[] SpeedDirection = new double[] { 0,0 };


		/// <summary>
		/// momentum
		/// </summary>
		public double[] Momentum = new double[] { 0, 0 };

		public bool StoreComplexValues = true;

		/// <summary>
		/// 3d rotation using float[3],
		/// these values should be between -1 and 1
		/// </summary>
		public float[] RotDirection = new float[] { 0, 0 };

		internal PhysicsMeshStorage PhysicsMeshStorage = new PhysicsMeshStorage();

		public _2dPhysicsParams() { }
		public _2dPhysicsParams(float massToSet, SimulatedObject[] collidableObjects) 
		{ 
			Mass = massToSet; 
			CollidableObjects = collidableObjects; 
		}
		public _2dPhysicsParams(float massToSet, float speed, double[] speedDirection, SimulatedObject[] collidableObjects)
		{
			Speed = speed;
			Mass = massToSet;
			SpeedDirection = speedDirection;
			CollidableObjects = collidableObjects;
		}
		public _2dPhysicsParams(float massToSet, float speed, double[] speedDirection, float[] rotDirection, SimulatedObject[] collidableObjects)
		{
			Speed = speed;
			Mass = massToSet;
			SpeedDirection = speedDirection;
			RotDirection = rotDirection;
			CollidableObjects = collidableObjects;
		}
	}
}