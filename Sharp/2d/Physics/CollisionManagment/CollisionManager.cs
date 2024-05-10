using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;
using Sharp.Utilities.MathUtils;
using Sharp.Utilities.MathUtils.DelaunayTriangulator;
using Sharp.Utilities.MISC;

namespace Sharp._2d.Physics.CollisionManagement
{
	public static class _2dCollisionManager
	{


		/// <summary>
		/// Checks if a object has collided with another object.
		/// </summary>
		/// <param name="hitables"></param>
		/// <param name="objectToCheck"></param>
		/// <param name="objectsPhysicsMeshStorage"></param>
		/// <returns></returns>
		public static CollisionData[] CheckIfCollidedWithObject(SimulatedObject2d[] hitables, SimulatedObject2d objectToCheck)
		{
			Point pnt;
			Triangle tri;
			bool result;
			List<CollisionData> ToReturn = [];
			foreach (SimulatedObject2d objectToCheckIfCollided in hitables)
			{
				try
				{
					for (int i = 0; i < objectToCheck.ObjectMesh.MeshTriangles.Length; i++)
					{
						tri = objectToCheck.ObjectMesh.MeshTriangles[i];
						for (int j = 0; j < objectToCheckIfCollided.ObjectMesh.MeshPoints.Length; j++)
						{
							pnt = objectToCheckIfCollided.ObjectMesh.MeshPoints[j];
							Point.AddNoCheck2D(pnt, objectToCheckIfCollided.Translation.ObjectPosition);
							result = MeshUtilities.IsInside(tri.Vertex1, tri.Vertex2, tri.Vertex3, pnt);
							if (result)
							{
								ToReturn.Add(new(tri, pnt, objectToCheckIfCollided));
							}
						}
					}
				}
				catch (NullReferenceException e)
				{
					// 6
					ErrorHandler.ThrowError(6, false);
					try
					{
						objectToCheck.ObjectMesh.MeshTriangles = [.. DelaunayTriangulator.DelaunayTriangulation(objectToCheck.ObjectMesh.MeshPoints)];
					}
					catch
					{
						//7
						ErrorHandler.ThrowError(7, true);
					}
				}
			}
			return [.. ToReturn];
		}

		/// <summary>
		/// Simulates a collision
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="meshLineIndex"></param>
		/// <param name="linePoint"></param>
		internal static void SimulateCollision(ref SimulatedObject2d obj, Triangle triCollided, Point pnt, ref SimulatedObject2d collidedObject)
		{

		}
	}
}