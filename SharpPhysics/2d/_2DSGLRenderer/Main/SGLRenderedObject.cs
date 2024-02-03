using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.Objects;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;

namespace SharpPhysics._2d._2DSGLRenderer.Main
{
	public class SGLRenderedObject
	{
		public uint BoundVao;
		public uint vbo;

		/// <summary>
		/// Object triangles
		/// </summary>
		public Triangle[] triangles;

		/// <summary>
		/// Mesh to render
		/// </summary>
		public Mesh Mesh = _2dBaseObjects.LoadSquareMesh();
	}
}
