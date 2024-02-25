using SharpPhysics._2d._2DSGLESRenderer.Shaders;
using SharpPhysics.Renderer._2DSGLESRenderer.Shaders;

namespace SharpPhysics._2d._2DSGLESRenderer.Shaders
{
	public class ShaderProgram
	{
		public Shader Vrtx;
		public Shader Frag;
		public uint ProgramPtr;

		public ShaderProgram()
		{
			Vrtx = new Shader();
			Frag = new Shader();
		}
	}
}
