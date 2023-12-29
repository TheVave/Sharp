using SharpPhysics._2d.ObjectRepresentation;

namespace SharpPhysics._2d.Physics.CollisionManagement
{
	public class CollisionData
	{
		public CollisionData(int collidedObjectMeshIndex, int collided2ObjectMeshIndex, _2dSimulatedObject collidedObject)
		{
			ObjectToCheckIfCollidedMeshIndex = collidedObjectMeshIndex;
			objectToCheckMeshIndex = collided2ObjectMeshIndex;
			CollidedObject = collidedObject;
		}

		public int ObjectToCheckIfCollidedMeshIndex { get; set; } = 0;
		public int objectToCheckMeshIndex { get; set; } = 0;
		public _2dSimulatedObject CollidedObject { get; set; } = new();
    }
}
