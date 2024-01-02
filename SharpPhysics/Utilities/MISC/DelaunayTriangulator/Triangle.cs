using SharpPhysics._2d.ObjectRepresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Utilities.MISC.DelaunayTriangulator
{

	public class Triangle
	{
		public Point Vertex1 { get; }
		public Point Vertex2 { get; }
		public Point Vertex3 { get; }

		public Edge[] Edges { get; }

		public Triangle(Point vertex1, Point vertex2, Point vertex3)
		{
			Vertex1 = vertex1;
			Vertex2 = vertex2;
			Vertex3 = vertex3;

			Edges = new Edge[] { new Edge(Vertex1, Vertex2), new Edge(Vertex2, Vertex3), new Edge(Vertex3, Vertex1) };
		}

		public bool IsPointInsideCircumcircle(Point point)
		{
			double ax = Vertex1.X - point.X;
			double ay = Vertex1.Y - point.Y;
			double bx = Vertex2.X - point.X;
			double by = Vertex2.Y - point.Y;
			double cx = Vertex3.X - point.X;
			double cy = Vertex3.Y - point.Y;

			double ab = ax * by - ay * bx;
			double bc = bx * cy - by * cx;
			double ca = cx * ay - cy * ax;

			double alift = ax * ax + ay * ay;
			double blift = bx * bx + by * by;
			double clift = cx * cx + cy * cy;

			// Check if point is inside the circumcircle
			return (alift * bc + blift * ca + clift * ab) > 0;
		}
	}
}
