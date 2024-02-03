using SharpPhysics.Utilities.MISC.Errors;
using System.Reflection;
using System.Resources;

namespace SharpPhysics._2d._2DSGLRenderer.Shaders
{
	public static class ShaderCollector
	{
		public static string GetShader(string name)
		{
			if (name == "VrtxNPs")
			{
				return "#version 330 core\r\n\r\nlayout (location = 0) in vec3 aPosition;\r\n\r\nvoid main()\r\n{\r\n    gl_Position = vec4(aPosition, 1.0);\r\n}";
			}
			if (name == "FragSnglClr")
			{
				return "#version 330 core\r\n\r\nout vec4 out_color;\r\n\r\nvoid main()\r\n{\r\n    out_color = vec4(1.0, 0.5, 0.2, 1.0);\r\n}";
			}
			ErrorHandler.ThrowError("Error, Internal Error, Missing resource.", true);
			return "how did you get here?";
		}
	}
}