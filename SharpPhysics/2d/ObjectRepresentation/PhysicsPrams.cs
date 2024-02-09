
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SharpPhysics._2d.ObjectRepresentation
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
		/// Increases the accuracy of the physics simulation but decreases the performance
		/// </summary>
		public int TicksPerSecond = 20;

		/// <summary>
		/// The resistance for moving
		/// </summary>
		public double SpeedResistance = 0.02;

		/// <summary>
		/// The objects that the object to simulate can collide with
		/// </summary>
		public _2dSimulatedObject[] CollidableObjects = [];

		/// <summary>
		/// The objects that the object is linked with, so that it will follow the motion of the parent.
		/// </summary>
		public _2dSimulatedObject[] LinkedObjects = [];

		/// <summary>
		/// the time multiplier
		/// </summary>
		public double TimeMultiplier = 1;

		/// <summary>
		/// Mass of the object
		/// </summary>
		public double Mass = 1f;

		/// <summary>
		/// The acceleration of the object in SpeedDirection. 
		/// Not recommended to be used, may have strange side effects.
		/// </summary>
		public double SpeedAcceleration = 0;

		/// <summary>
		/// the rotation resistance of the object
		/// </summary>
		public double RotResistance = 0.05;

		/// <summary>
		/// Speed in SpeedDirection
		/// </summary>
		public double Speed = 1;

		/// <summary>
		/// 3d direction using double[2]
		/// eg. SpeedDirection = new double { 10,0 }
		/// the object would go ten units in the x direction and 10 in the y direction
		/// </summary>
		public double[] Acceleration = [0, 0];


		/// <summary>
		/// momentum
		/// </summary>
		public double[] Momentum = [0, 0];

		/// <summary>
		/// The acceleration that the object will be experiencing.
		/// </summary>
		public double RotationalAcceleration = 0;

		/// <summary>
		/// The rotational momentum of the object
		/// Treated very similar to regular momentum.
		/// </summary>
		public double RotationalMomentum = 0;

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
	}
}