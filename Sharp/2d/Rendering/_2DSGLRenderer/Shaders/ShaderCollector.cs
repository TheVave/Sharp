using Sharp._2d.Rendering._2DSGLRenderer.Shaders;
using Sharp.StrangeDataTypes;
using Sharp.Utilities.MISC;

namespace Sharp._2d._2DSGLRenderer.Shaders
{
	public static class ShaderCollector
	{
		public static ShaderNamePair[] Pairs = [new()];

		public static string GetShader(string name)
		{
			string str;
			try
			{
				str = File.ReadAllText($"{Environment.CurrentDirectory}\\Shaders\\{name}.glsl");
				if (name.StartsWith("Frag"))
				{
					if (Pairs[^1].FragName != null)
					{
						ShaderNamePair[] buf = Pairs;
						Pairs = new ShaderNamePair[buf.Length + 1];
						Array.Copy(buf, Pairs, buf.Length);
						Pairs[^1] = new();
					}
					Pairs[^1].FragName = name;
				}
				else if (name.StartsWith("Vertex"))
				{
					if (Pairs[^1].VertName != null)
					{
						ShaderNamePair[] buf = Pairs;
						Pairs = new ShaderNamePair[buf.Length + 1];
						Array.Copy(buf, Pairs, buf.Length);
					}
					Pairs[^1].VertName = name;
				}
				else
				{
					// Error, Internal/External error, invalid shader type, or incorrect name, must start with Frag or Vertex.
					// The other shader types are not currently implemented, such as compute.
					// This may change in the future.
					ErrorHandler.ThrowError(10, true);
				}
				return str;
			}
			catch (IndexOutOfRangeException ioore)
			{
				ErrorHandler.ThrowError(20, true);
			}
			catch (FileNotFoundException)
			{
				//2
				ErrorHandler.ThrowError(2, true);
			}
			catch (DirectoryNotFoundException)
			{
				ErrorHandler.ThrowError(21, true);
			}
			// For C# to not get angry ||
			//                         ||
			//                         \/
			return "how did you get here?";
		}
	}
}