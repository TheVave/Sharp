using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;

namespace Sharp.Utilities.MathUtils.DelaunayTriangulator
{
	static public class DelaunayTriangulator
	{
		public static List<Triangle> DelaunayTriangulation(Point[] points)
		{
			List<Triangle> triangles = [];

			// Find bounding box
			double minX = points[0].X, minY = points[0].Y, maxX = points[0].X, maxY = points[0].Y;

			for (int i = 1; i < points.Length; i++)
			{
				double x = points[i].X, y = points[i].Y;

				if (x < minX) minX = x;
				if (y < minY) minY = y;
				if (x > maxX) maxX = x;
				if (y > maxY) maxY = y;
			}

			double dx = maxX - minX;
			double dy = maxY - minY;
			double deltaMax = Math.Max(dx, dy);
			double midx = (minX + maxX) / 2.0;
			double midy = (minY + maxY) / 2.0;

			Point superVertex1 = new(midx - 2 * deltaMax, midy - deltaMax);
			Point superVertex2 = new(midx + 2 * deltaMax, midy - deltaMax);
			Point superVertex3 = new(midx, midy + 2 * deltaMax);

			triangles.Add(new Triangle(superVertex1, superVertex2, superVertex3));

			int iteration = 0;

			foreach (var point in points)
			{
				List<Triangle> badTriangles = [];
				List<Edge> polygon = [];

				// Find bad triangles
				foreach (var triangle in triangles)
				{
					if (Triangle.IsPointInsideCircumcircle(point, triangle))
					{
						badTriangles.Add(triangle);

						foreach (var edge in triangle.Edges)
						{
							if (!polygon.Contains(edge))
							{
								polygon.Add(edge);
							}
						}
					}
				}

				triangles.RemoveAll(badTriangles.Contains);

				foreach (var edge in polygon)
				{
					triangles.Add(new Triangle(edge.Vertex1, edge.Vertex2, point));
				}

				iteration++;
			}

			triangles.RemoveAll(t =>
				Triangle.HasVertex(superVertex1, t) || Triangle.HasVertex(superVertex2, t) || Triangle.HasVertex(superVertex3, t));

			bool arg = triangles[1].Equals(triangles[2]);

			return [.. Triangle.MyDistinct([.. triangles])];
		}
	}
}