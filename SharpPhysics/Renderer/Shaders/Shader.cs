using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;
using GLFW;
using SharpPhysics.Utilities.MISC.Errors;

namespace SharpPhysics.Renderer.Shaders
{
	public class Shader
	{
		public string VertexCode;
		public string FragmentCode;

		public uint ProgramID;

		public Shader(string vertexCode, string fragmentCode)
		{
			VertexCode = vertexCode;
			FragmentCode = fragmentCode;
			this.Load();
		}


		public virtual void Load() 
		{
			uint vs = glCreateShader(GL_VERTEX_SHADER);
			glShaderSource(vs, VertexCode);
			glCompileShader(vs);

			uint fs = glCreateShader(GL_FRAGMENT_SHADER);
			glShaderSource(fs, FragmentCode);
			glCompileShader(fs);

			int[] vsstatus = glGetShaderiv(vs, GL_COMPILE_STATUS, 1);
			int[] fsstatus = glGetShaderiv(fs, GL_COMPILE_STATUS, 1);

			if (vsstatus[0] == 0 || fsstatus[0] == 0)
			{
				ErrorHandler.ThrowError("External/internal error, Shader compilation failed.\n" +
					" You may have supplied incorrect data for the fragment/vertex shader code. " +
					"\nThe shaders are programmed in HLSL.", false);
				ErrorHandler.ThrowError("Complete error (fs): " + glGetShaderInfoLog(fs), false);
				ErrorHandler.ThrowError("Complete error (vs): " + glGetShaderInfoLog(vs), true);
			}

			ProgramID = glCreateProgram();
			glAttachShader(ProgramID, vs);
			glAttachShader(ProgramID, fs);

			glLinkProgram(ProgramID);

			// detaching unneeded opengl mem
			glDetachShader(ProgramID, vs);
			glDetachShader(ProgramID, fs);
			glDeleteShader(vs);
			glDeleteShader(fs);
		}

		public void Use()
		{
			glUseProgram(ProgramID);
		}
	}
}
