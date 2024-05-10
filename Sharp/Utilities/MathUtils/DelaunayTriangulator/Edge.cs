using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;
using Sharp.Utilities.MISC.Unsafe;

namespace Sharp.Utilities.MathUtils.DelaunayTriangulator
{
	public class Edge(Point vertex1, Point vertex2) : ISizeGettable, IAny
	{
		public Point Vertex1 { get; } = vertex1;
		public Point Vertex2 { get; } = vertex2;

		public bool Equals(Edge other)
		{
			return Vertex1.Equals(other.Vertex1) && Vertex2.Equals(other.Vertex2) ||
				   Vertex1.Equals(other.Vertex2) && Vertex2.Equals(other.Vertex1);
		}

		public int GetSize()
		{
			unsafe
			{
				return sizeof(Edge);
			}
		}
	}
}
