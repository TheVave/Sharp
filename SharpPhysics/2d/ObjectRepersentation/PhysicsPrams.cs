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
		public _2dSimulatedObject[] CollidableObjects = Array.Empty<_2dSimulatedObject>();

		/// <summary>
		/// The objects that the object is linked with, so that it will follow the motion of the parent.
		/// </summary>
		public _2dSimulatedObject[] LinkedObjects = Array.Empty<_2dSimulatedObject>();

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
		public double SpeedResistance = 0.1;

		/// <summary>
		/// The acceleration of the object in SpeedDirection. 
		/// Not recommended to be used, may have strange side effects.
		/// </summary>
		public double SpeedAcceleration = 0;

		/// <summary>
		/// the rotation resistance of the object
		/// </summary>
		public double RotResistance = 1;

		/// <summary>
		/// Speed in SpeedDirection
		/// </summary>
		public double Speed = 1;

		/// <summary>
		/// 3d direction using double[2]
		/// eg. SpeedDirection = new double { 10,0 }
		/// the object would go ten units in the x direction and 10 in the y direction
		/// </summary>
		public double[] Acceleration = new double[] { 0,0 };


		/// <summary>
		/// momentum
		/// </summary>
		public double[] Momentum = new double[] { 0, 0 };

		// was going to be used when I was first
		// thinking up how to do collisions.
		//public bool StoreComplexValues = true;

		/// <summary>
		/// 3d rotation using float[3],
		/// these values should be between -1 and 1
		/// </summary>
		public float[] RotDirection = new float[] { 0, 0 };

		public _2dPhysicsParams() { }
		public _2dPhysicsParams(float massToSet, _2dSimulatedObject[] collidableObjects) 
		{ 
			Mass = massToSet; 
			CollidableObjects = collidableObjects; 
		}
		public _2dPhysicsParams(float massToSet, float speed, double[] speedDirection, _2dSimulatedObject[] collidableObjects)
		{
			Speed = speed;
			Mass = massToSet;
			Acceleration = speedDirection;
			CollidableObjects = collidableObjects;
		}
		public _2dPhysicsParams(float massToSet, float speed, double[] speedDirection, float[] rotDirection, _2dSimulatedObject[] collidableObjects)
		{
			Speed = speed;
			Mass = massToSet;
			Acceleration = speedDirection;
			RotDirection = rotDirection;
			CollidableObjects = collidableObjects;
		}
	}
}