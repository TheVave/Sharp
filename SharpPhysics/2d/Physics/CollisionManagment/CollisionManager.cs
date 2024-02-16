using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using SharpPhysics.Utilities.MISC;
using SharpPhysics.Utilities.MISC.Errors;

namespace SharpPhysics._2d.Physics.CollisionManagement
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
		public static CollisionData[] CheckIfCollidedWithObject(_2dSimulatedObject[] hitables, _2dSimulatedObject objectToCheck)
		{
			Point pnt;
			Triangle tri;
			bool result;
			List<CollisionData> ToReturn = [];
			foreach (_2dSimulatedObject objectToCheckIfCollided in hitables)
			{
				try
				{
					for (int i = 0; i < objectToCheck.ObjectMesh.MeshTriangles.Length; i++)
					{
						tri = objectToCheck.ObjectMesh.MeshTriangles[i];
						for (int j = 0; j < objectToCheckIfCollided.ObjectMesh.MeshPoints.Length; j++)
						{
							pnt = objectToCheckIfCollided.ObjectMesh.MeshPoints[j];
							pnt.AddNoCheck(objectToCheckIfCollided.Translation.ObjectPosition);
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
					ErrorHandler.ThrowError("Internal Error, objects triangles or points not initialized, attempting auto-correct.", false);
					try
					{
						objectToCheck.ObjectMesh.MeshTriangles = DelaunayTriangulator.DelaunayTriangulation(objectToCheck.ObjectMesh.MeshPoints).ToArray();
					}
					catch
					{
						ErrorHandler.ThrowError("Internal Error, objectToCheck points not initialized, mesh missing.", true);
					}
				}
			}
			return ToReturn.ToArray();
		}

		/// <summary>
		/// Simulates a collision
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="meshLineIndex"></param>
		/// <param name="linePoint"></param>
		internal static void SimulateCollision(ref _2dSimulatedObject obj, Triangle triCollided, Point pnt, ref _2dSimulatedObject collidedObject)
		{
			
		}
	}
}