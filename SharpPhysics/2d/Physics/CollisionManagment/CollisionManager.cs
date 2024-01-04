
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC;
using SharpPhysics.Utilities.MISC.Errors;

namespace SharpPhysics._2d.Physics.CollisionManagement
{
	public static class _2dCollisionManager
	{
		/// <summary>
		/// the point to find if this is inside the triangle
		/// </summary>
		private static Point a;

		/// <summary>
		/// The result from MeshUtils.IsInside
		/// </summary>
		private static bool result;

		/// <summary>
		/// point 1 forming the triangle for MeshUtils.IsInside
		/// </summary>
		private static Point b;

		/// <summary>
		/// point 2 forming the triangle for MeshUtils.IsInside
		/// </summary>
		private static Point c;

		/// <summary>
		/// point 3 forming the triangle for MeshUtils.IsInside
		/// </summary>
		private static Point d;

		/// <summary>
		/// The value to return from CheckIfCollidedWithObject
		/// </summary>

		private static bool hasBeenCollision;

		private static int index;


		/// <summary>
		/// Checks if a object has collided with another object.
		/// </summary>
		/// <param name="hitables"></param>
		/// <param name="objectToCheck"></param>
		/// <param name="objectsPhysicsMeshStorage"></param>
		/// <returns></returns>
		public static CollisionData[] CheckIfCollidedWithObject(_2dSimulatedObject[] hitables, _2dSimulatedObject objectToCheck)
		{
			hasBeenCollision = false;
			Span<_2dSimulatedObject> collidedObjects = Array.Empty<_2dSimulatedObject>();
			Span<int> objectToCheckMeshIndexes = Array.Empty<int>();
			Span<int> objectToCheckIfCollidedMeshIndices = Array.Empty<int>();
			Span<bool> isInsides;
			Span<CollisionData> ToReturn = Array.Empty<CollisionData>();
			foreach (_2dSimulatedObject objectToCheckIfCollided in hitables)
			{
				// resets the IsInside array for a new object
				isInsides = new bool[objectToCheckIfCollided.ObjectMesh.MeshPointsX.Length * objectToCheck.ObjectMesh.MeshPointsX.Length];
				index = 0;
				for (int i = 0; i < objectToCheckIfCollided.ObjectMesh.MeshPointsX.Length /* -2 for index errors because mesh points x.length + 2 is outside of the array */ - 2; i++)
				{
					for (int j = 0; j < objectToCheck.ObjectMesh.MeshPointsX.Length; j++)
					{
						try
						{
							// the point
							a = new Point(objectToCheck.ObjectMesh.MeshPointsX[j], objectToCheck.ObjectMesh.MeshPointsY[j]);
						}
						catch (NullReferenceException e)
						{
							ErrorHandler.ThrowError("Warning, Uninitialized values. Attempting auto-correct", false);
							try
							{
								objectToCheck.ObjectMesh.MeshPoints = new Point[objectToCheck.ObjectMesh.MeshPointsX.Length];
								for (int l = 0; l < objectToCheck.ObjectMesh.MeshPoints.Length; l++)
								{
									objectToCheck.ObjectMesh.MeshPoints[l] =
										new Point(objectToCheck.ObjectMesh.MeshPointsX[l], objectToCheck.ObjectMesh.MeshPointsY[l]);
								}
							}
							catch
							{
								ErrorHandler.ThrowError("Unable to correct. The program will close after you close this.", true);
							}
							finally
							{
								a = objectToCheck.ObjectMesh.MeshPoints[j];
							}
						}

						try
						{
							// triangle section a
							b = objectToCheckIfCollided.ObjectMesh.MeshPoints[i];

							// triangle section b
							c = objectToCheckIfCollided.ObjectMesh.MeshPoints[i + 1];

							// triangle section c
							d = objectToCheckIfCollided.ObjectMesh.MeshPoints[i + 2];
						}
						catch (NullReferenceException e)
						{
							ErrorHandler.ThrowError("Warning, Uninitialized values. Attempting auto-correct", false);
							try
							{
								objectToCheckIfCollided.ObjectMesh.MeshPoints = new Point[objectToCheckIfCollided.ObjectMesh.MeshPointsX.Length];
								for (int l = 0; l < objectToCheckIfCollided.ObjectMesh.MeshPoints.Length; l++)
								{
									objectToCheckIfCollided.ObjectMesh.MeshPoints[l] = 
										new Point(objectToCheckIfCollided.ObjectMesh.MeshPointsX[l], objectToCheckIfCollided.ObjectMesh.MeshPointsY[l]);
								}
							}
							catch
							{
								ErrorHandler.ThrowError("The error could not be corrected. The program will exit after you close this window.", true);
							}
							finally
							{

							}
						}

						try
						{
							// updating the result value so the physics sim will not be calculating the MeshUtils.IsInside twice.
							result = MeshUtilities.IsInside(b.X, b.Y, c.X, c.Y, d.X, d.Y, a.X, a.Y);
						}
						catch
						{
							ErrorHandler.ThrowError("Error, Unknown error, mesh points may be incorrect. Retrying.", false);
							try
							{
								result = MeshUtilities.IsInside(b.X, b.Y, c.X, c.Y, d.X, d.Y, a.X, a.Y);
							}
							catch
							{
								ErrorHandler.ThrowError($"Error, Unknown error, mesh points may be incorrect. Checking point {a} with triangle {b},{c},{d}", false);
								ErrorHandler.ThrowError($"If these values are unexpected, please look over meshes.", true);
							}
						}

						// if there has been a collision then updates objectToCheckIfCollidedMeshIndex and objectToCheckMeshIndex + optimizes the IsInsides setting some
						if (result)
						{
							isInsides = ArrayUtils.AddSpanObject(isInsides, result);

							objectToCheckIfCollidedMeshIndices = ArrayUtils.AddSpanObject(objectToCheckIfCollidedMeshIndices, i);

							objectToCheckMeshIndexes = ArrayUtils.AddSpanObject(objectToCheckMeshIndexes, j);

							collidedObjects = ArrayUtils.AddSpanObject(collidedObjects, objectToCheckIfCollided);
						}
					}
				}

				foreach (bool calcCollision in isInsides)
				{

					// there has been a collision.
					if (calcCollision)
					{
						ToReturn = ArrayUtils.AddSpanObject(ToReturn, new CollisionData(objectToCheckIfCollidedMeshIndices[index], objectToCheckMeshIndexes[index], collidedObjects[index]));
						index++;
						hasBeenCollision = true;
					}

				}
			}
			if (hasBeenCollision) return ToReturn.ToArray();
			else return null;
		}
	}
}