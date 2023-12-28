using System;

namespace SharpPhysics
{
	public class PhysicsMeshStorage
	{
		internal Tuple<double, double, double>[,] PhysicsTriangles;
		internal Tuple<double,double, double>[,] PhysicsAngles;
		internal bool TriEmpty = true;
		internal bool AngEmpty = true;

		public PhysicsMeshStorage() { }
		public PhysicsMeshStorage(Tuple<double, double, double>[,] physicsTriangles)
		{
			PhysicsTriangles = physicsTriangles;
			TriEmpty = false;
		}
	}
}
