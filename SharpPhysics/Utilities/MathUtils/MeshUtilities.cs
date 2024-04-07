using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;

namespace SharpPhysics.Utilities.MathUtils
{
	public static class MeshUtilities
	{
		public static double CalculateDistFromPoint(_2dPosition position)
		{
			return Math.Abs(position.X) + Math.Abs(position.Y) + Math.Abs(position.Z);
		}
		public static double CalculateMaxDistFromCenter(Mesh mesh, _2dPosition placeholderPositionObject)
		{
			double maxDist = 0;
			for (int i = 0; i < mesh.MeshPointsX.Length; i++)
			{
				placeholderPositionObject.X = mesh.MeshPointsActualX[i];
				placeholderPositionObject.Y = mesh.MeshPointsActualY[i];
				placeholderPositionObject.Z = 0;
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

		public static double GetArea(Point a, Point b, Point c)
		{
			return Math.Abs((a.X * (b.Y - c.Y) +
							 b.X * (c.Y - a.Y) +
							 c.X * (a.Y - b.Y)) / 2);
		}

		public static bool IsInside(Point vrtx1, Point vrtx2, Point vrtx3, Point pnt)
		{
			double a1 = GetArea(vrtx1, vrtx2, vrtx3);
			double b = GetArea(pnt, vrtx2, vrtx3);
			double c = GetArea(pnt, vrtx1, vrtx3);
			double d = GetArea(pnt, vrtx1, vrtx2);

			return (a1 == b + c + d);
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

		/// <summary>
		/// Finds the closest triangle edge with a line
		/// </summary>
		/// <param name="tri"></param>
		/// <param name="pnt"></param>
		/// <returns>
		/// </returns>
		public static GetClosestTriangleLineReturn GetClosestTriangleLine(Triangle tri, Point pnt)
		{
			// possible triangles: (1,2,p),(2,3,p),(1,3,p) 

			List<Triangle> triangles = [new(tri.Vertex1, tri.Vertex2, pnt), new(tri.Vertex2, tri.Vertex3, pnt), new(tri.Vertex1, tri.Vertex3, pnt)];
			double leastArea = 0;
			double checkingArea;
			Triangle leastTri = new();
			int least = 0;
			foreach (Triangle triangle in triangles)
			{
				checkingArea = Triangle.GetArea(triangle);
				if (leastArea > checkingArea)
				{
					leastArea = checkingArea;
					leastTri = triangle;
				}
				least++;
			}

			if (least == 1)
			{
				return new(1, 2, leastTri);
			}
			else if (least == 2)
			{
				return new(2, 3, leastTri);
			}
			else
			{
				return new(3, 1, leastTri);
			}
		}

		//public static bool GetIfTrianglesRecent(Mesh meshToCheck)
		//{
		//	meshToCheck.
		//}
	}
}
