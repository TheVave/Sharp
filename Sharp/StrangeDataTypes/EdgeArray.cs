using Sharp.Utilities.MathUtils.DelaunayTriangulator;
using Sharp.Utilities.MISC.Unsafe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.StrangeDataTypes
{
	[StructLayout(LayoutKind.Sequential)]
	public class EdgeArray : ISizeGettable
	{
		public Edge[] Edges;

		public EdgeArray()
		{
			Edges = new Edge[0];
		}

		public EdgeArray(Edge[] edges)
		{
			Edges = edges;
		}

		public int GetSize() => Edges[0].GetSize() * Edges.Length;
	}
}
