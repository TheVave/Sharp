using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;

namespace SharpPhysics._2d.Physics.CollisionManagement
{
	[Serializable]
	public class CollisionData : IAny
	{
		public CollisionData(Triangle collidedTriangle, Point collidedPoint, SimulatedObject2d collidedObject)
		{
			CollidedTriangle = collidedTriangle;
			CollidedPoint = collidedPoint;
			CollidedObject = collidedObject;
		}

		public Triangle CollidedTriangle;
		public Point CollidedPoint;
		public SimulatedObject2d CollidedObject;
	}
}
