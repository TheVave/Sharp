using SharpPhysics._2d._2DSGLRenderer.Shaders;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.Objects;
using SharpPhysics.Renderer.Textures;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;

namespace SharpPhysics._2d._2DSGLRenderer.Main
{
	public class SGLRenderedObject
	{
		/// <summary>
		/// The vao that contains all the VRAM data
		/// </summary>
		public uint BoundVao;

		/// <summary>
		/// The vbo containing the buffer data
		/// </summary>
		public uint vbo;

		/// <summary>
		/// The fragment shader to use. Don't mess with this unless you know what you're doing.
		/// </summary>
		public string FragShader = ShaderCollector.GetShader("FragTextureSupport");

		/// <summary>
		/// The vertex shader to use. Don't mess with this unless you know what you're doing.
		/// </summary>
		public string VrtxShader = ShaderCollector.GetShader("VertexPositionTexture");

		/// <summary>
		/// The pointer to the object texture
		/// </summary>
		public uint TexturePtr;

		/// <summary>
		/// The shader program
		/// </summary>
		public ShaderProgram Program = new();

		/// <summary>
		/// The object's texture
		/// </summary>
		public string objTextureLoc = "Enemy Thing.png";

		/// <summary>
		/// the object to simulate
		/// </summary>
		public _2dSimulatedObject objToSim = new();
	}
}
