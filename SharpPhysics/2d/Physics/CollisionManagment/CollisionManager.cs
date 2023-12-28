
using System;

namespace SharpPhysics
{
    public static class _2dCollisionManager
    {
		// the point to determine which side of the line
		// it is on
		private static Point a;
		// point 1 on the line for MeshUtils.IsLeft
		private static Point b;
		// point 2 on the line for MeshUtils.IsLeft
		private static Point c;
		// the result from MeshUtils.IsLeft
		private static int result;
		// the index for IsLefts[]
		private static int idx;
		// an array containing the info returned from MeshUtils.IsLeft
		private static bool[] isLefts;
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
				isLefts = new bool[objectToCheckIfCollided.ObjectMesh.MeshPointsX.Length * objectToCheck.ObjectMesh.MeshPointsX.Length];
				for (int i = 0; i < objectToCheckIfCollided.ObjectMesh.MeshPointsX.Length - 1; i++)
				{
					for (int j = 0; j < objectToCheck.ObjectMesh.MeshPointsX.Length; j++)
					{
						a = new Point(objectToCheck.ObjectMesh.MeshPointsX[j], objectToCheck.ObjectMesh.MeshPointsY[j]);
						b = new Point(objectToCheckIfCollided.ObjectMesh.MeshPointsX[i], objectToCheckIfCollided.ObjectMesh.MeshPointsY[i]);
						c = new Point(objectToCheckIfCollided.ObjectMesh.MeshPointsX[i + 1], objectToCheckIfCollided.ObjectMesh.MeshPointsY[i + 1]);

						idx = (i * objectToCheck.ObjectMesh.MeshPointsX.Length) + j;
						result = MeshUtilites.IsLeft(a, b, c);

						isLefts[idx] = result >= 0;
					}

				}
			}
			return null;
        }
    }
}