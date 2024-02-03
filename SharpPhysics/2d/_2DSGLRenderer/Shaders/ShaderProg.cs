using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics._2d._2DSGLRenderer.Shaders
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
