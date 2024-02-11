using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;

namespace SharpPhysics._2d.Physics.CollisionManagement
{
	public class CollisionData
	{
		public CollisionData(Triangle collidedTriangle, Point collidedPoint, _2dSimulatedObject collidedObject)
		{
			CollidedTriangle = collidedTriangle;
			CollidedPoint = collidedPoint;
			CollidedObject = collidedObject;
		}

		public Triangle CollidedTriangle;
		public Point CollidedPoint;
		public _2dSimulatedObject CollidedObject;
	}
}
