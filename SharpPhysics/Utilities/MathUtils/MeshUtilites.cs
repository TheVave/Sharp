
using System;

namespace SharpPhysics
{
    public static class MeshUtilites
    {
        public static int CalculateDistFromPoint(_2dPosition position)
        {
            return (int)(Math.Abs(position.xPos) + Math.Abs(position.yPos) + Math.Abs(position.zPos));
        }
        public static int CalculateMaxDistFromCenter(Mesh mesh, _2dPosition placeholderPositionObject)
        {
            int maxDist = 0;
            for (int i = 0; i < mesh.MeshPointsX.Length; i++)
            {
                placeholderPositionObject.xPos = mesh.MeshPointsX[i];
                placeholderPositionObject.yPos = mesh.MeshPointsY[i];
                placeholderPositionObject.zPos = mesh.MeshPointsZ[i];
                if (CalculateDistFromPoint(placeholderPositionObject) > maxDist)
                    maxDist = CalculateDistFromPoint(placeholderPositionObject);
            }
            return maxDist;
        }

		/// <summary>
		/// Returns true if a point is on the right side of the line.
		/// False if on the other side.
		/// If the line is horizontal, true is up from the line.
		/// </summary>
		/// <param name="a">
		/// The point to see which side of the line it is on
		/// </param>
		/// <param name="b">
		/// Point a of the line
		/// </param>
		/// <param name="c">
		/// Point b of the line
		/// </param>
		/// <returns>
		/// 
		/// </returns>
		public static int IsLeft(Point a, Point b, Point c)
		{
			return (int)((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X));
		}
	}
}
