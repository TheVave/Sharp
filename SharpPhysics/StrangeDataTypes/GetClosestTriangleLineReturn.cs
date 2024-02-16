using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.StrangeDataTypes
{
	public struct GetClosestTriangleLineReturn
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
