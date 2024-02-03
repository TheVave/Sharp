using System.Reflection;
using System.Resources;

namespace SharpPhysics._2d._2DSGLRenderer.Shaders
{
	public static class ShaderCollector
	{
		static string GetShader(string name)
		{
			string resourceName = "SharpPhysics.shaders";

			Assembly assembly = Assembly.GetExecutingAssembly();

			ResourceManager resourceManager = new ResourceManager(resourceName, assembly);

			string shaderStr = resourceManager.GetString(name);

			return shaderStr;
		}
	}
}