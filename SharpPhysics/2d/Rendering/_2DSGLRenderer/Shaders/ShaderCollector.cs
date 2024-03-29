using SharpPhysics._2d.Rendering._2DSGLRenderer.Shaders;
using SharpPhysics.Utilities.MISC;

namespace SharpPhysics._2d._2DSGLRenderer.Shaders
{
	public static class ShaderCollector
	{
		public static List<ShaderNamePair> Pairs;

		public static string GetShader(string name)
		{
			string str;
			try
			{
				str = File.ReadAllText($"{Environment.CurrentDirectory}\\Shaders\\{name}.glsl");
				if (name.StartsWith("Frag"))
				{
					if (Pairs[Pairs.Count - 1].FragName != null)
					{
						List<ShaderNamePair> buf = Pairs;
						Pairs = new List<ShaderNamePair>(Pairs.Count + 1);
						buf.CopyTo(Pairs.ToArray());
					}
					Pairs[Pairs.Count - 1].FragName = name;
				}
				else if (name.StartsWith("Vertex"))
				{
					if (Pairs[Pairs.Count - 1].VertName != null)
					{
						List<ShaderNamePair> buf = Pairs;
						Pairs = new List<ShaderNamePair>(Pairs.Count + 1);
						buf.CopyTo(Pairs.ToArray());
					}
					Pairs[Pairs.Count - 1].VertName = name;
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