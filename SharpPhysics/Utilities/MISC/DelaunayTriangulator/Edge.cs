using SharpPhysics._2d.ObjectRepresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Utilities.MISC.DelaunayTriangulator
{
	public class Edge
	{
		public Point Vertex1 { get; }
		public Point Vertex2 { get; }

		public Edge(Point vertex1, Point vertex2)
		{
			Vertex1 = vertex1;
			Vertex2 = vertex2;
		}

		public bool Equals(Edge other)
		{
			return (Vertex1.Equals(other.Vertex1) && Vertex2.Equals(other.Vertex2)) ||
				   (Vertex1.Equals(other.Vertex2) && Vertex2.Equals(other.Vertex1));
		}
	}
}
