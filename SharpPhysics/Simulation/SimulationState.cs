namespace SharpPhysics.Simulation
{
	public enum SimulationState
	{
		/// <summary>
		/// Not simulated
		/// </summary>
		Off,

		/// <summary>
		/// Immovable
		/// </summary>
		Static,

		/// <summary>
		/// Simulates Gravity
		/// </summary>
		GravitySimulated,

		/// <summary>
		/// falls forever but can be collided with
		/// </summary>
		StaticGravitySimulated,

		/// <summary>
		/// uncollidable
		/// </summary>
		NonCollidable,

		/// <summary>
		/// Completly immovable, can be walked through
		/// </summary>
		StaticNonCollidable,

		/// <summary>
		/// the same as staticNonCollidable but can be collided with
		/// </summary>
		GravitySimulatedNonCollidable
	}
}