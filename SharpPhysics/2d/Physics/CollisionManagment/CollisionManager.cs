
namespace SharpPhysics
{
    public static class _2dCollisionManager
    {
		/// <summary>
		/// the point to find if this is inside the triangle
		/// </summary>
		private static Point a;

		/// <summary>
		/// point 1 forming the triangle for MeshUtils.IsLeft
		/// </summary>
		private static Point b;

		/// <summary>
		/// point 2 forming the triangle for MeshUtils.IsLeft
		/// </summary>
		private static Point c;

		/// <summary>
		/// point 3 forming the triangle for MeshUtils.
		/// </summary>
		private static Point d;

		/// <summary>
		/// an array containing the info returned from MeshUtils.IsLeft
		/// </summary>
		private static bool[] IsInsides;

		/// <summary>
		/// The index(s) that 
		/// </summary>
		private static int[] objectToCheckIfCollidedMeshIndex;

        /// <summary>
        /// Checks if a object has collided with another object.
		/// Currently under development.
        /// </summary>
        /// <param name="hitables"></param>
        /// <param name="objectToCheck"></param>
        /// <param name="objectsPhysicsMeshStorage"></param>
        /// <returns></returns>
        public static _2dSimulatedObject? CheckIfCollidedWithObject(_2dSimulatedObject[] hitables, _2dSimulatedObject objectToCheck)
        {
			foreach (_2dSimulatedObject objectToCheckIfCollided in hitables)
			{
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

						// gets the main result, if the point is inside the triangle, then this returns true.
						IsInsides[(i * objectToCheck.ObjectMesh.MeshPointsX.Length) + j] = MeshUtilities.IsInside(b.X,b.Y,c.X,c.Y,d.X,d.Y,a.X,a.Y);
					} 
				}

				foreach (bool calcCollision in IsInsides)
				{
					// there has been a collision.
					if (calcCollision)
					{
						// this does work. Tested

					}
				}
			}
			return null;
        }
    }
}