using SharpPhysics.Utilities.MISC.Errors;
using System.Numerics;
using static OpenGL.GL;

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

		public virtual void SetMatrix4x4(string uniformName, Matrix4x4 val)
		{
			int Uniformlocation = glGetUniformLocation(ProgramID, uniformName);
			glUniformMatrix4fv(Uniformlocation, 1, false, GetMatrix4x4Values(val));
		}

		private float[] GetMatrix4x4Values(Matrix4x4 m)
		{
			return new float[]
			{
		m.M11, m.M12, m.M13, m.M14,
		m.M21, m.M22, m.M23, m.M24,
		m.M31, m.M32, m.M33, m.M34,
		m.M41, m.M42, m.M43, m.M44
			};
		}
	}
}
