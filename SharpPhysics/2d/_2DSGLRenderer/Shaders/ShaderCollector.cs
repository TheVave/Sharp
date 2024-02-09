using SharpPhysics.Utilities.MISC.Errors;
using System.Reflection;
using System.Resources;

namespace SharpPhysics._2d._2DSGLRenderer.Shaders
{
	public static class ShaderCollector
	{
		public static string GetShader(string name)
		{
			string str;
			try
			{
				str = File.ReadAllText($"{Environment.CurrentDirectory}\\Shaders\\{name}.glsl");
				return str;
			}
			catch
			{
				ErrorHandler.ThrowError("Error, External/Internal Error, Missing resource.", true);
			}
			// For C# to not get angry ||
			//                         ||
			//                         \/
			return "how did you get here?";
		}
	}
}