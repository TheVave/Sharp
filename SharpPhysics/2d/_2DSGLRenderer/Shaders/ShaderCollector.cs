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
			if (name == "FragTxtr")
			{
				return " #version 330 core\r\n\r\n        // This in attribute corresponds to the out attribute we defined in the vertex shader.\r\n        in vec2 frag_texCoords;\r\n        \r\n        out vec4 out_color;\r\n\r\n        // Now we define a uniform value!\r\n        // A uniform in OpenGL is a value that can be changed outside of the shader by modifying its value.\r\n        // A sampler2D contains both a texture and information on how to sample it.\r\n        // Sampling a texture is basically calculating the color of a pixel on a texture at any given point.\r\n        uniform sampler2D uTexture;\r\n        \r\n        void main()\r\n        {\r\n            // We use GLSL's texture function to sample from the texture at the given input texture coordinates.\r\n            out_color = texture(uTexture, frag_texCoords);\r\n        }";
			}
			if (name == "VrtxTxtrSprt")
			{
				return "#version 330 core\r\n        \r\n        layout (location = 0) in vec3 aPosition;\r\n\r\n        // On top of our aPosition attribute, we now create an aTexCoords attribute for our texture coordinates.\r\n        layout (location = 1) in vec2 aTexCoords;\r\n\r\n        // Likewise, we also assign an out attribute to go into the fragment shader.\r\n        out vec2 frag_texCoords;\r\n        \r\n        void main()\r\n        {\r\n            gl_Position = vec4(aPosition, 1.0);\r\n\r\n            // This basic vertex shader does no additional processing of texture coordinates, so we can pass them\r\n            // straight to the fragment shader.\r\n            frag_texCoords = aTexCoords;\r\n        }";
			}
			ErrorHandler.ThrowError("Error, Internal Error, Missing resource.", true);
			return "how did you get here?";
		}
	}
}