using Sharp._2d._2DSGLRenderer.Shaders;
using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;
using Sharp.Utilities.MISC.Unsafe;

namespace Sharp._2d._2DSGLRenderer.Main
{
	public unsafe class SGLRenderedObject : ISizeGettable, IAny
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
		/// The contents of the ebo
		/// </summary>
		public uint[] eboContent;

		/// <summary>
		/// Pointer to the ebo in graphics mem
		/// </summary>
		public uint eboPtr;

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
		public string ObjectTextureLocation = string.Empty;

		/// <summary>
		/// the object to simulate
		/// </summary>
		public UnmanagedMemoryObject<SimulatedObject2d> objToSim;

		/// <summary>
		/// The rendered objects hash code.
		/// Significantly speeds up adding and removing objects.
		/// NOTE: DO NOT CHANGE!
		/// </summary>
		internal long HashCode;

		/// <summary>
		/// If the objToSim needs to be freed to prevent a mem leak once the object is disposed.
		/// </summary>
		internal bool NeedMemFreeSimulatedObject2d = true;

		/// <summary>
		/// If the renderer/other SP parts are loading info for the renderer.
		/// </summary>
		internal bool CurrentlyLoadingInfo = false;

		/// <summary>
		/// If the object has been initialized by Internal2dRenderer
		/// </summary>
		internal bool ObjectInitialized = false;

		~SGLRenderedObject()
		{
			// removes the UnsafeUtils.Malloc'd memory to prevent mem leak
			if (NeedMemFreeSimulatedObject2d)
				objToSim.Dispose();
		}

		public int GetSize() =>
			// simplest objects
			(sizeof(uint) * (4 + eboContent.Length))
			+ sizeof(bool) * 3
			+ sizeof(long)
			// strings
			+ UnsafeUtils.GetSimpleObjectSize(FragShader)
			+ UnsafeUtils.GetSimpleObjectSize(VrtxShader)
			+ UnsafeUtils.GetSimpleObjectSize(ObjectTextureLocation)
			// complex
			+ objToSim.GetSize()
			+ Program.GetSize();
	}
}
