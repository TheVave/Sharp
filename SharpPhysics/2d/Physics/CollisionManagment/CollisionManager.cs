
namespace SharpPhysics
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
		/// an array containing the info returned from MeshUtils.IsLeft
		/// </summary>
		private static bool[] IsInsides;

		/// <summary>
		/// The index(s) in the hitables array meshes that have collided with objectToCheck
		/// </summary>
		private static int[] objectToCheckIfCollidedMeshIndex = Array.Empty<int>();

		/// <summary>
		/// The index(s) in the objectToCheck mesh that have collided with hitables[x]
		/// </summary>
		private static int[] objectToCheckMeshIndex = Array.Empty<int>();

		/// <summary>
		/// All of the objects that collided with objectToCheck
		/// </summary>
		private static _2dSimulatedObject[] CollidedObjects = Array.Empty<_2dSimulatedObject>();

		/// <summary>
		/// The value to return from CheckIfCollidedWithObject
		/// </summary>
		private static Tuple<int, int, _2dSimulatedObject>[]? ToReturn = Array.Empty<Tuple<int, int, _2dSimulatedObject>>();

		private static bool hasBeenCollision;


		/// <summary>
		/// Checks if a object has collided with another object.
		/// Currently under development.
		/// </summary>
		/// <param name="hitables"></param>
		/// <param name="objectToCheck"></param>
		/// <param name="objectsPhysicsMeshStorage"></param>
		/// <returns></returns>
		public static Tuple<int,int,_2dSimulatedObject>[]? CheckIfCollidedWithObject(_2dSimulatedObject[] hitables, _2dSimulatedObject objectToCheck)
        {
			foreach (_2dSimulatedObject objectToCheckIfCollided in hitables)
			{
				// resets the IsInside array for a new object
				IsInsides = new bool[objectToCheckIfCollided.ObjectMesh.MeshPointsX.Length * objectToCheck.ObjectMesh.MeshPointsX.Length];
				for (int i = 0; i < objectToCheckIfCollided.ObjectMesh.MeshPointsX.Length /* -2 for index errors because mesh points x.length + 2 is outside of the array */ - 2; i++)
				{
					for (int j = 0; j < objectToCheck.ObjectMesh.MeshPointsX.Length; j++)
					{
						// the point
						a = new Point(objectToCheck.ObjectMesh.MeshPointsX[j], objectToCheck.ObjectMesh.MeshPointsY[j]);

						// triangle section a
						b = new Point(objectToCheckIfCollided.ObjectMesh.MeshPointsX[i/* + 0 */], objectToCheckIfCollided.ObjectMesh.MeshPointsY[i]/* + 0 */);

						// triangle section b
						c = new Point(objectToCheckIfCollided.ObjectMesh.MeshPointsX[i + 1], objectToCheckIfCollided.ObjectMesh.MeshPointsY[i + 1]);

						// triangle section c
						d = new Point(objectToCheckIfCollided.ObjectMesh.MeshPointsX[i + 2], objectToCheckIfCollided.ObjectMesh.MeshPointsY[i + 2]);

						// updating the result value so the physics sim will not be calculating the MeshUtils.IsInside twice.
						result = MeshUtilities.IsInside(b.X, b.Y, c.X, c.Y, d.X, d.Y, a.X, a.Y);

						// if there has been a collision then updates objectToCheckIfCollidedMeshIndex and objectToCheckMeshIndex + optimizes the IsInsides setting some
						if (result)
						{
							IsInsides[(i * objectToCheck.ObjectMesh.MeshPointsX.Length) + j] = result;

							objectToCheckIfCollidedMeshIndex = objectToCheckIfCollidedMeshIndex.Append(i).ToArray();

							objectToCheckMeshIndex = objectToCheckMeshIndex.Append(j).ToArray();

							CollidedObjects = CollidedObjects.Append(objectToCheckIfCollided).ToArray();
						}
					}
				}
				int indx = 0;

				foreach (bool calcCollision in IsInsides)
				{
					
					// there has been a collision.
					if (calcCollision)
					{
						ToReturn = ToReturn.Append(
							new Tuple<int, int, _2dSimulatedObject>
							(objectToCheckIfCollidedMeshIndex[indx], objectToCheckMeshIndex[indx], CollidedObjects[indx]))
							.ToArray();
						indx++;
						hasBeenCollision = true;
					}
					
				}
			}
			if (hasBeenCollision) return ToReturn;
			else return null;
        }
    }
}