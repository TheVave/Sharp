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
	public class TriangleArray : ISizeGettable
	{
		public Triangle[] Objects;

		public int GetSize() =>
			Objects[0].GetSize() * Objects.Length;

		public void FromTriangles(Triangle[] tri) =>
			Objects = tri;

		public TriangleArray()
		{
			Objects = new Triangle[0];
		}
		public TriangleArray(Triangle[] objects)
		{
			Objects = objects;
		}
	}
}
