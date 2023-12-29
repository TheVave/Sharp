namespace SharpPhysics
{
	public static class MeshUtilities
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

		// The following code is from GeeksForGeeks (https://www.geeksforgeeks.org/check-whether-a-given-point-lies-inside-a-triangle-or-not/)
		// ints replaced with doubles.

		/* A utility function to calculate area of triangle 
		formed by (x1, y1) (x2, y2) and (x3, y3) */
		public static double Area(double x1, double y1, double x2,
						   double y2, double x3, double y3)
		{
			return Math.Abs((x1 * (y2 - y3) +
							 x2 * (y3 - y1) +
							 x3 * (y1 - y2)) / 2.0);
		}

		/* A function to check whether point P(x, y) lies
		inside the triangle formed by A(x1, y1),
		B(x2, y2) and C(x3, y3) */
		public static bool IsInside(double x1, double y1, double x2,
							 double y2, double x3, double y3,
							 double x, double y)
		{
			/* Calculate area of triangle ABC */
			double A = Area(x1, y1, x2, y2, x3, y3);

			/* Calculate area of triangle PBC */
			double A1 = Area(x, y, x2, y2, x3, y3);

			/* Calculate area of triangle PAC */
			double A2 = Area(x1, y1, x, y, x3, y3);

			/* Calculate area of triangle PAB */
			double A3 = Area(x1, y1, x2, y2, x, y);

			/* Check if sum of A1, A2 and A3 is same as A */
			return (A == A1 + A2 + A3);
		}


		/// <summary>
		/// Calculate the area of a polygon from a sequence of points.
		/// </summary>
		/// <param name="polygon"></param>
		/// <returns></returns>
		// from https://stackoverflow.com/questions/2432428/is-there-any-algorithm-for-calculating-area-of-a-shape-given-co-ordinates-that-d
		public static double PolygonArea(Point[] polygon)
		{
			int i, j;
			double area = 0;

			for (i = 0; i < polygon.Length; i++)
			{
				j = (i + 1) % polygon.Length;

				area += polygon[i].X * polygon[j].Y;
				area -= polygon[i].Y * polygon[j].X;
			}

			area /= 2;
			return (area < 0 ? -area : area);
		}
	}
}
