using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;
using Sharp.Utilities.MathUtils.DelaunayTriangulator;

namespace Sharp._2d.Physics.CollisionManagement
{
	[Serializable]
	public class CollisionData(Triangle collidedTriangle, Point collidedPoint, SimulatedObject2d collidedObject) : IAny
	{
		public Triangle CollidedTriangle = collidedTriangle;
		public Point CollidedPoint = collidedPoint;
		public SimulatedObject2d CollidedObject = collidedObject;
	}
}
