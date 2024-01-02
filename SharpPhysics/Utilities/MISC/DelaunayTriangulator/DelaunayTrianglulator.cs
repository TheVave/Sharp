using SharpPhysics._2d.ObjectRepresentation;

namespace SharpPhysics.Utilities.MISC.DelaunayTriangulator
{
	static public class DelaunayTriangulator
	{
		public static List<Triangle> DelaunayTriangulation(Point[] points)
		{
			List<Triangle> triangles = new List<Triangle>();

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

			// Initialize super-triangle vertices closer to the input points
			double dx = maxX - minX;
			double dy = maxY - minY;
			double deltaMax = Math.Max(dx, dy);
			double midx = (minX + maxX) / 2.0;
			double midy = (minY + maxY) / 2.0;

			Point superVertex1 = new Point(midx - 2 * deltaMax, midy - deltaMax);
			Point superVertex2 = new Point(midx + 2 * deltaMax, midy - deltaMax);
			Point superVertex3 = new Point(midx, midy + 2 * deltaMax);

			triangles.Add(new Triangle(superVertex1, superVertex2, superVertex3));

			// Add each point to the triangulation
			for (int i = 0; i < points.Length; i++)
			{
				List<Triangle> badTriangles = new List<Triangle>();
				List<Edge> polygon = new List<Edge>();

				foreach (var triangle in triangles)
				{
					if (triangle.IsPointInsideCircumcircle(points[i]))
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

				foreach (var triangle in badTriangles)
				{
					triangles.Remove(triangle);
				}

				foreach (var edge in polygon)
				{
					triangles.Add(new Triangle(edge.Vertex1, edge.Vertex2, points[i]));
				}
			}
			foreach (var triangle in triangles)
			{
				Console.WriteLine($"Triangle: ({triangle.Vertex1.X}, {triangle.Vertex1.Y}), " +
								  $"({triangle.Vertex2.X}, {triangle.Vertex2.Y}), " +
								  $"({triangle.Vertex3.X}, {triangle.Vertex3.Y})");
			}

			// Remove super-triangle vertices
			triangles.RemoveAll(t =>
				t.Vertex1.Equals(superVertex1) || t.Vertex1.Equals(superVertex2) || t.Vertex1.Equals(superVertex3) ||
				t.Vertex2.Equals(superVertex1) || t.Vertex2.Equals(superVertex2) || t.Vertex2.Equals(superVertex3) ||
				t.Vertex3.Equals(superVertex1) || t.Vertex3.Equals(superVertex2) || t.Vertex3.Equals(superVertex3));

			return triangles;
		}
	}
}