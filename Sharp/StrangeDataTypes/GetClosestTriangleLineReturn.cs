using Sharp.Utilities.MathUtils.DelaunayTriangulator;

namespace Sharp.StrangeDataTypes
{
	public struct GetClosestTriangleLineReturn : IAny
	{
		public Triangle tri;
		public int vertex1;
		public int vertex2;

		public GetClosestTriangleLineReturn(int vertex1, int vertex2, Triangle tri)
		{
			this.tri = tri;
			this.vertex1 = vertex1;
			this.vertex2 = vertex2;
		}
	}
}
