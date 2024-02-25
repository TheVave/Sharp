using SharpPhysics.Utilities.MISC;

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
				//2
				ErrorHandler.ThrowError(2, true);
			}
			// For C# to not get angry ||
			//                         ||
			//                         \/
			return "how did you get here?";
		}
	}
}