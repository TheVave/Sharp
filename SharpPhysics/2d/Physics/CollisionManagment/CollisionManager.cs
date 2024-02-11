
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using SharpPhysics.Utilities.MISC;
using SharpPhysics.Utilities.MISC.Errors;
using System.ComponentModel;

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
			Span<_2dSimulatedObject> collidedObjects = Array.Empty<_2dSimulatedObject>();
			Span<Point> Points = Array.Empty<Point>();
			Span<Triangle> CollidedTriangles = Array.Empty<Triangle>();
			Span<bool> isInsides = [];
			Point pnt;
			Triangle tri;
			bool result;
			List<CollisionData> ToReturn = [];
			foreach (_2dSimulatedObject objectToCheckIfCollided in hitables)
			{
				// resets the IsInside array for a new object
				isInsides = new bool[objectToCheck.ObjectMesh.MeshTriangles.Length];

				try
				{
					for (int i = 0; i < objectToCheck.ObjectMesh.MeshTriangles.Length; i++)
					{
						tri = objectToCheck.ObjectMesh.MeshTriangles[i];
						tri = tri.RotateByRadians(GenericMathUtils.DegreesToRadians(objectToCheck.Translation.ObjectRotation.xRot));
						tri = tri.ShiftTriangle(new(objectToCheck.Translation.ObjectPosition.xPos, objectToCheck.Translation.ObjectPosition.yPos));
						for (int j = 0; j < objectToCheckIfCollided.ObjectMesh.MeshPoints.Length; j++)
						{
							pnt = objectToCheckIfCollided.ObjectMesh.MeshPoints[j];
							pnt += new Point(objectToCheckIfCollided.Translation.ObjectPosition.xPos, objectToCheckIfCollided.Translation.ObjectPosition.yPos);
							result = MeshUtilities.IsInside(tri.Vertex1.X, tri.Vertex1.Y, tri.Vertex2.X, tri.Vertex2.Y, tri.Vertex3.X, tri.Vertex3.Y, pnt.X, pnt.Y);
							if (result)
							{
								// there has been a collision
								isInsides = ArrayUtils.AddSpanObject(isInsides, true);

								Points = ArrayUtils.AddSpanObject(Points, pnt);

								CollidedTriangles = ArrayUtils.AddSpanObject(CollidedTriangles, tri);

								collidedObjects = ArrayUtils.AddSpanObject(collidedObjects, objectToCheckIfCollided);
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
			ToReturn = [];
			for (int i = 0; i < collidedObjects.Length; i++)
			{
				ToReturn.Add(new(CollidedTriangles[i], Points[i], collidedObjects[i]));
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