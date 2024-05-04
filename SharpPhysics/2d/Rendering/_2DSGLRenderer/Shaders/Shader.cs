using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d._2DSGLRenderer.Shaders
{
	public struct Shader : ISizeGettable, IAny
	{
		public uint Program;
		public string ShaderCode;
		public uint ShaderCompilePtr;
		public Silk.NET.OpenGL.ShaderType ShaderType;

		public override bool Equals(object obj)
		{
			Shader shader;
			try
			{
				shader = (Shader)obj;
			}
			catch
			{
				return false;
			}
			if (ShaderCode != shader.ShaderCode)
			{
				return false;
			}
			if (Program != shader.Program)
			{
				return false;
			}
			if (shader.ShaderCompilePtr != ShaderCompilePtr)
			{
				return false;
			}
			if (shader.ShaderType != ShaderType)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return ((Program.GetHashCode() / ShaderCode.GetHashCode()) / (int)ShaderCompilePtr);
		}

		public readonly int GetSize() =>
			// simple
			(sizeof(uint) * 2)
			// enums
			+ (sizeof(int))
			// strings
			+ UnsafeUtils.GetSimpleObjectSize(ShaderCode);

		public static bool operator ==(Shader left, Shader right)
		{
			if (left.ShaderCode != right.ShaderCode)
			{
				return false;
			}
			if (left.Program != right.Program)
			{
				return false;
			}
			if (left.ShaderCompilePtr != right.ShaderCompilePtr)
			{
				return false;
			}
			if (left.ShaderType != right.ShaderType)
			{
				return false;
			}
			return true;
		}

		public static bool operator !=(Shader left, Shader right)
		{
			return !(left == right);
		}
	}
}
