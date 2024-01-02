using Microsoft.VisualBasic;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MISC.Errors;

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
			int iteration = 0;

			foreach (var point in points)
			{
				List<Triangle> badTriangles = new List<Triangle>();
				List<Edge> polygon = new List<Edge>();

				// Find bad triangles
				foreach (var triangle in triangles)
				{
					if (triangle.IsPointInsideCircumcircle(point))
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

				// Remove bad triangles
				triangles.RemoveAll(triangle => badTriangles.Contains(triangle));

				// Add new triangles to the triangulation
				foreach (var edge in polygon)
				{
					triangles.Add(new Triangle(edge.Vertex1, edge.Vertex2, point));
				}

				iteration++;
			}

			// Remove triangles with super-triangle vertices
			triangles.RemoveAll(t =>
				t.HasVertex(superVertex1) || t.HasVertex(superVertex2) || t.HasVertex(superVertex3));

			return triangles.Distinct().ToList();
		}
	}
}