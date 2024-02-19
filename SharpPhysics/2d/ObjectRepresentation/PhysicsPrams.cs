namespace SharpPhysics._2d.ObjectRepresentation
{
	public class PhysicsParams2d
	{
		/// <summary>
		/// The resistance for moving
		/// TODO: this should be coefficient of friction instead (which there are multiple types)
		/// </summary>
		public double SpeedResistance = 0.002;

		/// <summary>
		/// the rotation resistance of the object3
		/// TODO: research moment of inertia as an alternative
		/// </summary>
		public double RotResistance = 0.05;

		/// <summary>
		/// What scene the object can collide with.
		/// </summary>
		public short sceneCollidableId;

		/// <summary>
		/// Mass of the object
		/// </summary>
		public double Mass = 1f;

		/// <summary>
		/// The scene objects the object can interact with.
		/// </summary>
		public short sceneID = 0;

		/// <summary>
		/// Speed in SpeedDirection
		/// </summary>
		public Vector2d Velocity = new(0, 0);

		/// <summary>
		/// The rotational momentum of the object
		/// Treated very similar to regular momentum.
		/// </summary>
		public double RotationalVelocity = 0;

		public PhysicsParams2d() { }
		public PhysicsParams2d(float mass)
		{
			Mass = mass;
		}
		public PhysicsParams2d(float mass, Vector2d velocity)
		{
			Velocity = velocity;
			Mass = mass;
		}
	}
}