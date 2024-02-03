using SharpPhysics._2d._2DSGLRenderer.Shaders;
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
		/// The fragment shader to use. Don't mess with this unless you know what you're doing.
		/// </summary>
		public string FragShader = ShaderCollector.GetShader("FragSnglClr");

		/// <summary>
		/// The vertex shader to use. Don't mess with this unless you know what you're doing.
		/// </summary>
		public string VrtxShader = ShaderCollector.GetShader("VrtxNPs");

		/// <summary>
		/// Mesh to render
		/// </summary>
		public Mesh Mesh = _2dBaseObjects.LoadSquareMesh();

		/// <summary>
		/// The shader program
		/// </summary>
		public ShaderProgram Program = new();
	}
}
