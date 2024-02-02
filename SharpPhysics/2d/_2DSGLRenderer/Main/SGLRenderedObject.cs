using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using System.Numerics;

namespace SharpPhysics._2d._2DSGLRenderer.Main
{
	public class SGLRenderedObject
	{
		public uint BoundVao;
		public uint vbo;

		public SharpPhysics._2d.ObjectRepresentation.Point[] objPoints;
		public Triangle[] triangles;
		

		public SGLRenderedObject() { }
	}
}
